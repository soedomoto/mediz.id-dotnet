// Google Sign-In callback function - will be set by Blazor component
window.handleCredentialResponse = function(response) {
    console.log("Google Sign-In successful, token received");
    
    if (response && response.credential) {
        // Call the Blazor HandleGoogleSignIn method
        if (window.blazorLoginComponent) {
            window.blazorLoginComponent.invokeMethodAsync('HandleGoogleSignIn', response.credential)
                .catch(err => console.error("Error calling HandleGoogleSignIn:", err));
        } else {
            console.error("Blazor login component reference not available");
        }
    }
};

// Initialize Google Sign-In
window.initializeGoogleSignIn = function() {
    try {
        if (window.google && window.google.accounts && window.google.accounts.id) {
            console.log("Google Sign-In library loaded successfully");
            // Initialize the Google SDK
            window.google.accounts.id.initialize({
                client_id: document.getElementById("g_id_onload")?.getAttribute("data-client_id") || "YOUR_GOOGLE_CLIENT_ID",
                callback: window.handleCredentialResponse
            });
        } else {
            console.error("Google Sign-In library not loaded");
        }
    } catch (error) {
        console.error("Error initializing Google Sign-In:", error);
    }
};

// Helper function to trigger Google Sign-In UI
window.triggerGoogleSignIn = function() {
    try {
        if (window.google && window.google.accounts && window.google.accounts.id) {
            window.google.accounts.id.prompt((notification) => {
                console.log("Google prompt notification:", notification);
            });
        }
    } catch (error) {
        console.error("Error triggering Google Sign-In:", error);
    }
};

