using MudBlazor;

namespace MedizID.Web.Services;

/// <summary>
/// Service for displaying notifications (toasts/snackbars)
/// Similar to React's @mantine/notifications
/// Provides a unified API for success, error, info, and warning notifications
/// </summary>
public class NotificationService
{
    private readonly ISnackbar _snackbar;

    public NotificationService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    /// <summary>Show a success notification</summary>
    public void Success(string title, string message = "")
    {
        var fullMessage = string.IsNullOrEmpty(message) ? title : $"{title}: {message}";
        _snackbar.Add(fullMessage, Severity.Success);
    }

    /// <summary>Show an error notification</summary>
    public void Error(string title, string message = "")
    {
        var fullMessage = string.IsNullOrEmpty(message) ? title : $"{title}: {message}";
        _snackbar.Add(fullMessage, Severity.Error);
    }

    /// <summary>Show an info notification</summary>
    public void Info(string title, string message = "")
    {
        var fullMessage = string.IsNullOrEmpty(message) ? title : $"{title}: {message}";
        _snackbar.Add(fullMessage, Severity.Info);
    }

    /// <summary>Show a warning notification</summary>
    public void Warning(string title, string message = "")
    {
        var fullMessage = string.IsNullOrEmpty(message) ? title : $"{title}: {message}";
        _snackbar.Add(fullMessage, Severity.Warning);
    }

    /// <summary>Show a custom notification with all parameters</summary>
    public void Show(string message, Severity severity = Severity.Normal)
    {
        _snackbar.Add(message, severity);
    }
}
