using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Mission.Entities.Entities;
using Mission.Entities.Models;
using Mission.Entities.Models.CommonModel;
using Mission.Entities.Models.MissionsModels;
using Mission.Services.IServices;
using Mission.Services.Services;
using System.Net.Http.Headers;

namespace Mission.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController(IMissionService missionService, IWebHostEnvironment hostingEnvironment) : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
        private readonly IMissionService _missionService = missionService;
        ResponseResult result = new ResponseResult();


        [HttpGet]
        [Route("GetMissionThemeList")]
        [Authorize]
        public ResponseResult MissionThemeList()
        {
            try
            {
                result.Data = _missionService.MissionThemeList();
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        [Route("GetMissionSkillList")]
        [Authorize]
        public ResponseResult MissionSkillList()
        {
            try
            {
                result.Data = _missionService.MissionSkillList();
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        [Route("MissionList")]
        public ResponseResult MissionList()
        {
            try
            {
                result.Data = _missionService.GetMissionList();
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        [Route("MissionDetailsById/{id}")]
        public ResponseResult MissionDetailsById(int id)
        {
            try
            {
                result.Data = _missionService.GetMissionById(id);
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }


        [HttpPost]
        [Route("AddMission")]
        public ResponseResult AddMission([FromForm] AddMissionRequestModel request)
        {
            try
            {
                if(request.EndDate.Date < request.StartDate.Date) { throw new Exception("Selected end date must be greater then start date"); };

                result.Data = _missionService.AddMission(request);
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("UpdateMission")]
        public async Task<IActionResult> UpdateMission([FromForm] MissionRequestViewModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest(new ResponseResult() { Result = ResponseStatus.Error, Message = "Invalid mission data" });

                var updateResult = await _missionService.UpdateMission(model);

                if (!updateResult)
                    return NotFound(new ResponseResult() { Result = ResponseStatus.Error, Message = "Mission not found or not updated" });

                return Ok(new ResponseResult()
                {
                    Data = "Mission updated successfully",
                    Result = ResponseStatus.Success
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult()
                {
                    Result = ResponseStatus.Error,
                    Message = ex.Message
                });
            }
        }


        [HttpDelete]
        [Route("DeleteMission/{id}")]
        public ResponseResult DeleteMission(int id)
        {
            try
            {
                result.Data = _missionService.DeleteMission(id);
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("MissionApplicationApprove")]
        public ResponseResult MissionApplicationApprove(MissionApplication application)
        {
            try
            {
                result.Data = _missionService.ApproveMission(application.Id);
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        [Route("MissionApplicationList")]
        public ResponseResult MissionApplicationList()
        {
            try
            {
                result.Data = _missionService.MissionApplicationList();
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }


        [HttpPost]
        [Route("MissionApplicationDelete")]
        public ResponseResult MissionApplicationDelete(MissionApplication application)
        {
            try
            {
                result.Data = _missionService.DeleteMissionApplication(application.Id);
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }





    }
}
