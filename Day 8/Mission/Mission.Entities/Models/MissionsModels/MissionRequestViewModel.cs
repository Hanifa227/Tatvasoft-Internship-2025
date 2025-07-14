using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Mission.Entities.Models.MissionsModels;

public class MissionRequestViewModel
{
    public int Id { get; set; }
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public string MissionTitle { get; set; }
    public int MissionThemeId { get; set; }
    public string MissionThemeName { get; set; } = string.Empty;
    public string MissionDescription { get; set; }

    public int TotalSeats { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string MissionImages { get; set; }
    public string MissionSkillId { get; set; }
    public string MissionOrganisationName { get; set; }
    public string MissionOrganisationDetail { get; set; }
    public string MissionType { get; set; }
}
