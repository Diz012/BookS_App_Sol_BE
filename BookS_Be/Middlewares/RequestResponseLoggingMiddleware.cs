using System.Text;
using System.Text.Json;

namespace BookS_Be.Middlewares;

/// <summary>
/// Middleware to log incoming requests and outgoing responses with detailed information and colors
/// </summary>
public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Log incoming request
        await LogRequestAsync(context);

        // Capture the original response body stream
        var originalBodyStream = context.Response.Body;

        try
        {
            // Create a new memory stream to capture response
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Continue down the middleware pipeline
            await next(context);

            // Log outgoing response
            await LogResponseAsync(context, responseBody);

            // Copy the response back to the original stream
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
        finally
        {
            // Restore the original response body stream
            context.Response.Body = originalBodyStream;
        }
    }

    private async Task LogRequestAsync(HttpContext context)
    {
        try
        {
            var request = context.Request;
            
            // Enable buffering so we can read the body multiple times
            request.EnableBuffering();

            // Capture request body
            var requestBody = string.Empty;
            if (request.ContentLength > 0)
            {
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0; // Reset position for next middleware
            }

            // Extract query parameters
            var queryParams = new Dictionary<string, string>();
            foreach (var param in request.Query)
            {
                queryParams[param.Key] = param.Value.ToString();
            }

            // Get request method styling
            var (requestEmoji, requestColor) = GetRequestMethodStyle(request.Method);

            // Create simplified log message
            var queryString = queryParams.Count > 0 ? $" | Query: {string.Join(", ", queryParams.Select(kv => $"{kv.Key}={kv.Value}"))}" : "";
            var bodyPreview = !string.IsNullOrEmpty(requestBody) && requestBody.Length > 200 
                ? $" | Body: {requestBody[..200]}..." 
                : !string.IsNullOrEmpty(requestBody) ? $" | Body: {requestBody}" : "";

            logger.LogInformation("{RequestColor}{RequestEmoji} [REQUEST] {Method} {Path}{QueryString}{BodyPreview}{ResetColor}", 
                requestColor, requestEmoji, request.Method, request.Path.Value, queryString, bodyPreview, "\u001b[0m");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Error logging request");
        }
    }

    private async Task LogResponseAsync(HttpContext context, MemoryStream responseBody)
    {
        try
        {
            var response = context.Response;
            
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            // Parse response body to extract error details if it's an error
            string? errorMessage = null;
            string? responsePreview = null;

            if (!string.IsNullOrEmpty(responseBodyText))
            {
                // Create response preview (first 200 chars)
                responsePreview = responseBodyText.Length > 200 ? $"{responseBodyText[..200]}..." : responseBodyText;
                
                try
                {
                    using var jsonDoc = JsonDocument.Parse(responseBodyText);
                    
                    // Try to extract error message from common error response patterns
                    if (response.StatusCode >= 400)
                    {
                        if (jsonDoc.RootElement.TryGetProperty("message", out var messageElement))
                            errorMessage = messageElement.GetString();
                        else if (jsonDoc.RootElement.TryGetProperty("error", out var errorElement))
                            errorMessage = errorElement.GetString();
                        else if (jsonDoc.RootElement.TryGetProperty("title", out var titleElement))
                            errorMessage = titleElement.GetString();
                        else
                            errorMessage = responsePreview;
                    }
                }
                catch
                {
                    if (response.StatusCode >= 400)
                        errorMessage = responsePreview;
                }
            }

            var statusDescription = GetStatusDescription(response.StatusCode);
            var isError = response.StatusCode >= 400;
            var (responseEmoji, responseColor) = GetResponseStatusStyle(response.StatusCode);

            // Create simplified log message
            var errorInfo = isError && !string.IsNullOrEmpty(errorMessage) ? $" | Error: {errorMessage}" : "";
            var responseInfo = !isError && !string.IsNullOrEmpty(responsePreview) && response.ContentType?.Contains("application/json") == true 
                ? $" | Response: {responsePreview}" : "";
            var sizeInfo = $" | Size: {responseBody.Length} bytes";

            var logLevel = isError ? LogLevel.Warning : LogLevel.Information;
            
            logger.Log(logLevel, "{ResponseColor}{ResponseEmoji} [RESPONSE] {StatusCode} {StatusDescription}{ErrorInfo}{ResponseInfo}{SizeInfo}{ResetColor}", 
                responseColor, responseEmoji, response.StatusCode, statusDescription, errorInfo, responseInfo, sizeInfo, "\u001b[0m");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Error logging response");
        }
    }

    private static (string emoji, string color) GetRequestMethodStyle(string method)
    {
        return method.ToUpper() switch
        {
            "GET" => ("🔍", "\u001b[34m"),      // Blue - Reading/Searching
            "POST" => ("➕", "\u001b[32m"),     // Green - Creating
            "PUT" => ("✏️", "\u001b[33m"),      // Yellow - Updating
            "PATCH" => ("🔧", "\u001b[36m"),    // Cyan - Partial update
            "DELETE" => ("🗑️", "\u001b[31m"),   // Red - Deleting
            "OPTIONS" => ("⚙️", "\u001b[35m"),  // Magenta - Options
            "HEAD" => ("👁️", "\u001b[90m"),     // Gray - Head request
            _ => ("📨", "\u001b[37m")           // White - Default
        };
    }

    private static (string emoji, string color) GetResponseStatusStyle(int statusCode)
    {
        return statusCode switch
        {
            // Success responses (2xx) - Green shades
            >= 200 and < 300 => statusCode switch
            {
                200 => ("✅", "\u001b[32m"),    // Bright Green - OK
                201 => ("🎉", "\u001b[92m"),    // Light Green - Created
                202 => ("⏳", "\u001b[93m"),    // Yellow - Accepted
                204 => ("🆗", "\u001b[36m"),    // Cyan - No Content
                _ => ("✅", "\u001b[32m")       // Default success
            },
            
            // Redirection responses (3xx) - Blue shades
            >= 300 and < 400 => ("🔄", "\u001b[34m"),    // Blue - Redirect
            
            // Client error responses (4xx) - Orange/Yellow shades
            >= 400 and < 500 => statusCode switch
            {
                400 => ("⚠️", "\u001b[93m"),     // Yellow - Bad Request
                401 => ("🔐", "\u001b[33m"),     // Orange - Unauthorized
                403 => ("🚫", "\u001b[91m"),     // Light Red - Forbidden
                404 => ("🔍", "\u001b[35m"),     // Magenta - Not Found
                409 => ("💥", "\u001b[95m"),     // Light Magenta - Conflict
                422 => ("📝", "\u001b[93m"),     // Yellow - Validation Error
                _ => ("⚠️", "\u001b[93m")        // Default client error
            },
            
            // Server error responses (5xx) - Red shades
            >= 500 => statusCode switch
            {
                500 => ("❌", "\u001b[31m"),     // Red - Internal Server Error
                502 => ("🌐", "\u001b[91m"),     // Light Red - Bad Gateway
                503 => ("🔧", "\u001b[31m"),     // Red - Service Unavailable
                _ => ("💀", "\u001b[31m")        // Dark Red - Critical error
            },
            
            _ => ("❓", "\u001b[37m")            // White - Unknown
        };
    }

    private static string GetStatusDescription(int statusCode)
    {
        return statusCode switch
        {
            200 => "OK - Success",
            201 => "Created - Resource created successfully",
            202 => "Accepted - Request accepted for processing",
            204 => "No Content - Success with no response body",
            301 => "Moved Permanently - Resource moved",
            302 => "Found - Temporary redirect",
            400 => "Bad Request - Invalid request data",
            401 => "Unauthorized - Authentication required",
            403 => "Forbidden - Access denied",
            404 => "Not Found - Resource not found",
            409 => "Conflict - Resource already exists",
            422 => "Unprocessable Entity - Validation failed",
            500 => "Internal Server Error - Server error occurred",
            502 => "Bad Gateway - Gateway error",
            503 => "Service Unavailable - Service temporarily unavailable",
            _ => $"HTTP {statusCode}"
        };
    }
}
