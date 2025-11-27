using MedizID.Web.Services.Generated;
using MedizID.Web.Services.Generated.Models;
using Vizor.Icons.Tabler;

public class FacilityLayoutState
{
    private bool _isAuthenticated = false;
    public bool IsAuthenticated 
    { 
        get => _isAuthenticated;
        set 
        { 
            if (_isAuthenticated != value)
            {
                _isAuthenticated = value;
                OnAuthenticationChanged?.Invoke(value);
            }
        }
    }
    public event Action<bool>? OnAuthenticationChanged;
    public bool IsAuthenticating = false;
    private FacilityResponse? _facility;
    public FacilityResponse? Facility
    {
        get => _facility;
        set
        {
            _facility = value;
            OnFacilityChanged?.Invoke(value);
        }
    }
    public event Action<FacilityResponse?>? OnFacilityChanged;
    public Dictionary<string, (string Title, IconData? Icon)> Tabs = new()
    {
        [""] = ("Home", TablerSvgIcon.BuildingHospital),
        ["installation"] = ("Installation", TablerSvgIcon.BuildingFactory),
        ["polyclinic"] = ("Polyclinic", TablerSvgIcon.BuildingHospital),
        ["staff"] = ("Staff", TablerSvgIcon.Users),
        ["patient"] = ("Patient", TablerSvgIcon.User),
        ["appointment"] = ("Appointment", TablerSvgIcon.Calendar),
    };
    public string _activeTabKey = "";
    public string ActiveTabKey
    {
        get => _activeTabKey;
        set
        {
            _activeTabKey = value;
            OnActiveTabKeyChanged?.Invoke(value);
        }
    }
    public event Action<string?>? OnActiveTabKeyChanged;

    public async Task FetchFacilityAsync(MedizIdApiClient ApiClient, Guid FacilityId)
    {
        Facility = await ApiClient.Api.V1.Facilities[FacilityId].GetAsync();
    }
}