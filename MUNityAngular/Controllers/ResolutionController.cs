using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNityAngular.Services;
using MUNityAngular.Util.Extenstions;
using MUNityAngular.Models.Resolution;
using MUNityAngular.Schema.Request;
using MUNityAngular.Hubs.HubObjects;
using MUNityAngular.DataHandlers.EntityFramework.Models;
using MUNityAngular.Models.Resolution.V2;
using MUNityAngular.Schema.Request.Resolution;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResolutionController : ControllerBase
    {
        IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> _hubContext;

        public ResolutionController(IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ResolutionV2> CreatePublic(string title, [FromServices]IResolutionService service)
        {
            if (User == null)
            {
                // This should be called when the User is not logged in
            }

            return await service.CreatePublicResolution(title);
        }

        [Route("[action]")]
        [HttpPatch]
        [Authorize]
        public async Task<ResolutionV2> UpdateProtectedResolution([FromBody] ResolutionV2 resolution,
            [FromServices]IAuthService authService,
            [FromServices]IResolutionService resolutionService)
        {
            var user = authService.GetUserOfClaimPrincipal(User);
            // Check if user is allowed to update this document
            var canEdit = authService.CanUserEditResolution(user, resolution);

            if (!canEdit)
                return null;

            // Update this document
            return await resolutionService.SaveResolution(resolution);
        }

        /// <summary>
        /// Updates a Resolution that has Public Access. 
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="resolutionService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [AllowAnonymous]
        public async Task<ActionResult<ResolutionV2>> UpdatePublicResolution([FromBody] ResolutionV2 resolution,
            [FromServices] IResolutionService resolutionService)
        {
            // Check if a resolution with this Id exists
            var res = await resolutionService.GetResolution(resolution.ResolutionId);

            if (res == null)
                return NotFound("Resolution not found");

            // Check if the resolution is public
            var auth = await resolutionService.GetResolutionAuth(resolution.ResolutionId);
            if (auth.AllowPublicEdit)
            {
                // Update this document
                var updatedDocument = await resolutionService.SaveResolution(resolution);
                return Ok(updatedDocument);
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Something went wrong when saving the resolution!");
        }

        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ResolutionV2> GetPublic(string id,
            [FromServices] IResolutionService resolutionService)
        {
            return await resolutionService.GetResolution(id);
        }

        ///// <summary>
        ///// Creates a new Resolution
        ///// </summary>
        ///// <param name="auth">The authenticator-code </param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns>The new created Resolution in a json format.</returns>
        //[Route("[action]")]
        //[HttpGet]
        //public ActionResult<HUBResolution> CreatePublic([FromHeader]string auth, [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{

        //    if (string.IsNullOrEmpty(auth))
        //        return StatusCode(StatusCodes.Status200OK, resolutionService.CreatePublicResulution());

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanCreateResolution(user))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");


        //    ResolutionModel resolution = resolutionService.CreatePrivateResolution(user);
        //    return StatusCode(StatusCodes.Status200OK, new HUBResolution(resolution));
        //}

        //[Route("[action]")]
        //[HttpGet]
        //public ActionResult<HUBResolution> CreatePublic(
        //    [FromServices] ResolutionService resolutionService)
        //{
        //    ResolutionModel resolution = resolutionService.CreatePublicResulution();
        //    return Ok(new HUBResolution(resolution));
        //}

        ///// <summary>
        ///// Adds a new Preamble Paragraph to a resolutions and returns its object.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPost]
        //public ActionResult<HUBPreambleParagraph> AddPreambleParagraph([FromBody]ChangeResolutionRequest body,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{

        //    var resolution = resolutionService.GetResolution(body.ResolutionId);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfClaimPrincipal(User);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var newParagraph = resolution.Preamble.AddParagraph();
        //    resolutionService.RequestSave(resolution);
        //    _hubContext.Clients.Group(body.ResolutionId).PreambleParagraphAdded(resolution.Preamble.Paragraphs.IndexOf(newParagraph), newParagraph.ID, newParagraph.Text);
        //    return StatusCode(StatusCodes.Status200OK, new HUBPreambleParagraph(newParagraph));
        //}

        ///// <summary>
        ///// Adds an Operative Paragraph to the Resolution.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpGet]
        //public ActionResult<HUBOperativeParagraph> AddOperativeParagraph([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");


        //    var newPP = resolution.AddOperativeParagraph();
        //    resolutionService.RequestSave(resolution);
        //    var hubModel = new HUBOperativeParagraph(newPP);
        //    _hubContext.Clients.Group(resolutionid).OperativeParagraphAdded(resolution.OperativeSections.IndexOf(newPP), hubModel);
        //    return StatusCode(StatusCodes.Status200OK, hubModel);


        //}

        ///// <summary>
        ///// Changes the value of the Preamble paragraph. Use this function if the text has
        ///// changed or you want to add a comment.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="paragraph"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPut]
        //public ActionResult<HUBPreambleParagraph> UpdatePreambleParagraph([FromHeader]string auth, [FromBody]Hubs.HubObjects.HUBPreambleParagraph paragraph,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(paragraph.ResolutionID);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var pp = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.ID == paragraph.ID);
        //    if (pp == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Preamble Paragraph not found!");

        //    pp.Text = paragraph.Text;
        //    var hubElement = new HUBPreambleParagraph(pp);
        //    _hubContext.Clients.Group(resolution.ID).PreambleParagraphChanged(hubElement);
        //    resolutionService.RequestSave(resolution);

        //    return StatusCode(StatusCodes.Status200OK, hubElement);
        //}

        ///// <summary>
        ///// Updates an Operative Section. Use this Request when changing the text of an
        ///// operative parahraph or you want to add a comment.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="paragraph"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPut]
        //public ActionResult<HUBOperativeParagraph> UpdateOperativeSection([FromHeader]string auth, [FromBody]HUBOperativeParagraph paragraph,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{

        //    var resolution = resolutionService.GetResolution(paragraph.ResolutionID);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var section = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraph.ID);
        //    if (section == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Operative Section not found!");

        //    section.Text = paragraph.Text;
        //    //section.Notices = paragraph.Notices;
        //    var hubModel = new HUBOperativeParagraph(section);
        //    _hubContext.Clients.Group(resolution.ID).OperativeParagraphChanged(hubModel);
        //    resolutionService.RequestSave(resolution);
        //    return StatusCode(StatusCodes.Status200OK, hubModel);


        //}

        //[Route("[action]")]
        //[HttpPatch]
        //public ActionResult<HUBOperativeParagraph> UpdateOperativeSectionNotices([FromHeader]string auth, [FromBody]HUBOperativeParagraph paragraph,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(paragraph.ResolutionID);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var section = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraph.ID);
        //    if (section == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Operative Section not found!");

        //    paragraph.Notices.ForEach(n =>
        //    {
        //        if (string.IsNullOrWhiteSpace(n.Id))
        //        {
        //            n.Id = Guid.NewGuid().ToString();
        //        }
        //    });

        //    section.Notices = paragraph.Notices;
        //    var hubModel = new HUBOperativeParagraph(section);
        //    _hubContext.Clients.Group(resolution.ID).OperativeParagraphChanged(hubModel);
        //    resolutionService.RequestSave(resolution);
        //    return StatusCode(StatusCodes.Status200OK, hubModel);
        //}

        //[Route("[action]")]
        //[HttpPatch]
        //public ActionResult<HUBOperativeParagraph> UpdatePreambleParagraphNotices([FromHeader]string auth, [FromBody]HUBPreambleParagraph paragraph,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(paragraph.ResolutionID);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var pp = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.ID == paragraph.ID);
        //    if (pp == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Preamble Paragraph not found!");

        //    paragraph.Notices.ForEach(n =>
        //    {
        //        if (string.IsNullOrWhiteSpace(n.Id))
        //        {
        //            n.Id = Guid.NewGuid().ToString();
        //        }
        //    });

        //    pp.Notices = paragraph.Notices;
        //    var hubModel = new HUBPreambleParagraph(pp);
        //    _hubContext.Clients.Group(resolution.ID).PreambleParagraphChanged(hubModel);
        //    resolutionService.RequestSave(resolution);
        //    return StatusCode(StatusCodes.Status200OK, hubModel);
        //}

        //[Route("[action]")]
        //[HttpPut]
        //public ActionResult<NoticeModel> UpdateOperativeSectionNotice([FromHeader]string auth,
        //    [FromHeader]string resolutionid,
        //    [FromHeader]string paragraphid,
        //    [FromBody]NoticeModel notice,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var section = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
        //    if (section == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Operative Section not found!");

        //    var foundNotice = section.Notices.FirstOrDefault(n => n.Id == notice.Id);
        //    //Update
        //    if (foundNotice != null)
        //    {
        //        foundNotice.Text = notice.Text;
        //        foundNotice.Tags = notice.Tags;
        //        foundNotice.ReadBy = notice.ReadBy;
        //    }
        //    else
        //    {
        //        //Add
        //        notice.Id = Guid.NewGuid().ToString();
        //        notice.CreationDate = DateTime.Now;
        //        if (user != null)
        //        {
        //            notice.AuthorName = user.Forename + " " + user.Lastname;
        //            notice.AuthorId = user.Username;
        //        }
        //        section.Notices.Add(notice);
        //    }

        //    var hubModel = new HUBOperativeParagraph(section);
        //    _hubContext.Clients.Group(resolution.ID).OperativeParagraphChanged(hubModel);
        //    resolutionService.RequestSave(resolution);
        //    return StatusCode(StatusCodes.Status200OK, notice);
        //}

        //[Route("[action]")]
        //[HttpPut]
        //public ActionResult<NoticeModel> UpdatePreambleParagraphNotice([FromHeader]string auth,
        //    [FromHeader]string resolutionid,
        //    [FromHeader]string paragraphid,
        //    [FromBody]NoticeModel notice,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var paragraph = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.ID == paragraphid);
        //    if (paragraph == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Preamble Paragraph not found!");

        //    var foundNotice = paragraph.Notices.FirstOrDefault(n => n.Id == notice.Id);
        //    //Update
        //    if (foundNotice != null)
        //    {
        //        foundNotice.Text = notice.Text;
        //        foundNotice.Tags = notice.Tags;
        //        foundNotice.ReadBy = notice.ReadBy;
        //    }
        //    else
        //    {
        //        //Add
        //        notice.Id = Guid.NewGuid().ToString();
        //        notice.CreationDate = DateTime.Now;
        //        if (user != null)
        //        {
        //            notice.AuthorName = user.Forename + " " + user.Lastname;
        //            notice.AuthorId = user.Username;
        //        }
        //        paragraph.Notices.Add(notice);
        //    }

        //    var hubModel = new HUBPreambleParagraph(paragraph);
        //    _hubContext.Clients.Group(resolution.ID).PreambleParagraphChanged(hubModel);
        //    resolutionService.RequestSave(resolution);
        //    return StatusCode(StatusCodes.Status200OK, notice);
        //}

        ///// <summary>
        ///// Removes a preamble paragraph from the resolution
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="paragraphid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpDelete]
        //public IActionResult RemovePreambleParagraph([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string paragraphid,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var paragraph = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.ID == paragraphid);
        //    if (paragraph == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

        //    resolution.Preamble.Paragraphs.Remove(paragraph);
        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Group(resolutionid).PreambleParaghraphRemoved(resolution.Preamble.Paragraphs.ToHubParagraphs());
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// moves the preamble paragraph one step up if thats possible
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="paragraphid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPatch]
        //public IActionResult MovePreambleParagraphUp([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string paragraphid,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var paragraph = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.ID == paragraphid);
        //    if (paragraph == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

        //    paragraph.MoveUp();
        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Groups(resolutionid).PreambleSectionOrderChanged(resolution.Preamble.Paragraphs.ToHubParagraphs());

        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Moves the preamble paragraph one stop down if thats possible
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="paragraphid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPatch]
        //public IActionResult MovePreambleParahraphDown([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string paragraphid,
        //    [FromServices] ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var paragraph = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.ID == paragraphid);
        //    if (paragraph == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

        //    paragraph.MoveDown();
        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Groups(resolutionid).PreambleSectionOrderChanged(resolution.Preamble.Paragraphs.ToHubParagraphs());
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Moves the operative paragraph one step up if thats possible
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="paragraphid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPatch]
        //public IActionResult MoveOperativeParagraphUp([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string paragraphid,
        //    [FromServices] ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);

        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var paragraph = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
        //    if (paragraph == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

        //    paragraph.MoveUp();
        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Groups(resolutionid).OperativeSectionOrderChanged(resolution.OperativeSections.ToHubParagraphs());
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Moves the operative paragraph one step down
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="paragraphid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPatch]
        //public IActionResult MoveOperativeParagraphDown([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string paragraphid,
        //    [FromServices] ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var paragraph = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
        //    if (paragraph == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

        //    paragraph.MoveDown();
        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Groups(resolutionid).OperativeSectionOrderChanged(resolution.OperativeSections.ToHubParagraphs());
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Moves the paragraph to the left so it loses one level.
        ///// This means that if the OA is 1.3.2 it will become 1.4.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="paragraphid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPatch]
        //public IActionResult MoveOperativeParagraphLeft([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string paragraphid,
        //    [FromServices] ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var paragraph = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
        //    if (paragraph == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

        //    paragraph.MoveLeft();
        //    resolutionService.RequestSave(resolution);
        //    _hubContext.Clients.Groups(resolutionid).OperativeSectionOrderChanged(resolution.OperativeSections.ToHubParagraphs());
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Moves the Operative Paragraph on step to the right. This means it will become a
        ///// sub Point of the next OA that is the same level as this one the moment before this
        ///// action is called.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="paragraphid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPatch]
        //public IActionResult MoveOperativeParagraphRight([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string paragraphid,
        //    [FromServices] ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var paragraph = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
        //    if (paragraph == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

        //    paragraph.MoveRight();
        //    resolutionService.RequestSave(resolution);
        //    _hubContext.Clients.Groups(resolutionid).OperativeSectionOrderChanged(resolution.OperativeSections.ToHubParagraphs());
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Removes a operative paragraph and all amendments that are linked to it.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="paragraphid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpDelete]
        //public IActionResult RemoveOperativeParagraph([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string paragraphid,
        //    [FromServices] ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var paragraph = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
        //    if (paragraph == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

        //    paragraph.Remove();
        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Groups(resolutionid).OperativeParagraphRemoved(new HUBResolution(resolution));
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        //#region Document Information and Options

        //[Route("[action]")]
        //[HttpPatch]
        //public IActionResult ChangeHeader([FromHeader]string auth, [FromBody]UpdateResolutionHeaderRequest request,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(request.ResolutionId);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    resolution.Topic = request.Title;
        //    resolution.SubmitterName = request.SubmitterName;
        //    resolution.CommitteeName = request.Committee;
        //    resolution.SupporterNames = request.Supporters;
        //    resolutionService.RequestSave(resolution);
        //    _hubContext?.Clients.Group(resolution.ID).ResolutionChanged(new Hubs.HubObjects.HUBResolution(resolution));
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Changes the Public Read Mode of the resolution
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="pmode"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPatch]
        //public ActionResult<string> ChangePublicReadMode([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string pmode,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{

        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    bool newMode = false;
        //    if (pmode.ToLower().Trim() == "true" || pmode == "1")
        //        newMode = true;

        //    var code = resolutionService.SetPublicReadMode(resolutionid, newMode);
        //    return StatusCode(StatusCodes.Status200OK, code);
        //}

        //#endregion

        //#region Amendments

        ///// <summary>
        ///// Adds a new DeleteAmendment to the resolution
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="sectionid"></param>
        ///// <param name="submittername"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPost]
        //public IActionResult AddDeleteAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string sectionid, [FromHeader]string submittername,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var section = resolution.OperativeSections.FirstOrDefault(n => n.ID == sectionid);
        //    if (section == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Operative Paragraph Not found!");

        //    var amendment = new DeleteAmendmentModel();
        //    amendment.SubmitterName = submittername.DecodeUrl();
        //    amendment.TargetSection = section;

        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Group(resolutionid).DeleteAmendmentAdded(new Hubs.HubObjects.HUBDeleteAmendment(amendment));
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Adds a new Change Text Amendment to the resolution
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="model"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPost]
        //public IActionResult AddChangeAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromBody]Hubs.HubObjects.HUBChangeAmendment model,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{

        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var section = resolution.OperativeSections.FirstOrDefault(n => n.ID == model.TargetSectionID);
        //    if (section == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Operative Paragraph Not found!");

        //    var amendment = new ChangeAmendmentModel();
        //    amendment.SubmitterName = model.SubmitterName;
        //    amendment.NewText = model.NewText;
        //    amendment.TargetSection = section;

        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Group(resolutionid).ChangeAmendmentAdded(new Hubs.HubObjects.HUBChangeAmendment(amendment));
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Adds a new Move Operative Paragraph Amendment to the resolution
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="sectionid"></param>
        ///// <param name="submittername"></param>
        ///// <param name="newposition"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPost]
        //public IActionResult AddMoveAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string sectionid, [FromHeader]string submittername, [FromHeader]string newposition,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{

        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var section = resolution.OperativeSections.FirstOrDefault(n => n.ID == sectionid);
        //    if (section == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Operative Paragraph Not found!");

        //    if (int.TryParse(newposition, out int np))
        //    {
        //        var amendment = new MoveAmendmentModel();
        //        amendment.SubmitterName = submittername.DecodeUrl();
        //        amendment.NewPosition = np;
        //        amendment.TargetSection = section;

        //        resolutionService.RequestSave(resolution);

        //        _hubContext.Clients.Group(resolutionid).MoveAmendmentAdded(new Hubs.HubObjects.HUBResolution(resolution), new Hubs.HubObjects.HUBMoveAmendment(amendment));
        //        return StatusCode(StatusCodes.Status200OK);
        //    }
        //    return StatusCode(StatusCodes.Status406NotAcceptable, "The new Position has to be a number!");
        //}

        ///// <summary>
        ///// Adds a new Add-Paragraph Amendment to the resolution.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="amendment"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPost]
        //public IActionResult AddAddAmendment([FromHeader]string auth, [FromBody]HUBAddAmendment amendment,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{

        //    var resolution = resolutionService.GetResolution(amendment.TargetResolutionID);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var model = new AddAmendmentModel();
        //    model.SubmitterName = amendment.SubmitterName;
        //    model.TargetPosition = amendment.TargetPosition;
        //    model.NewText = amendment.NewText;
        //    model.TargetResolution = resolution;

        //    resolutionService.RequestSave(resolution);

        //    //The amendment gets its ResolutionId From the Controller thats why we create a new model.
        //    _hubContext.Clients.Group(resolution.ID).AddAmendmentAdded(new Hubs.HubObjects.HUBResolution(resolution), new Hubs.HubObjects.HUBAddAmendment(model));
        //    return StatusCode(StatusCodes.Status200OK);

        //}

        ///// <summary>
        ///// Removes a given Amendment
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="amendmentid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpDelete]
        //public IActionResult RemoveAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string amendmentid,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var amendment = resolution.Amendments.FirstOrDefault(n => n.ID == amendmentid);
        //    if (amendment == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound, "Amendment not found");
        //    }

        //    amendment.Remove();
        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Group(resolutionid).AmendmentRemoved(new Hubs.HubObjects.HUBResolution(resolution), new Hubs.HubObjects.HUBAbstractAmendment(amendment));
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Activates the given Amendment on the resolution.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="amendmentid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPatch]
        //public IActionResult ActivateAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string amendmentid,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var amendment = resolution.Amendments.FirstOrDefault(n => n.ID == amendmentid);
        //    if (amendment == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound, "Amendment not found");
        //    }

        //    amendment.Activate();
        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Group(resolutionid).AmendmentActivated(new Hubs.HubObjects.HUBResolution(resolution), new Hubs.HubObjects.HUBAbstractAmendment(amendment));
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Deactivates the given amendment on the resolution.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="amendmentid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPatch]
        //public IActionResult DeactivateAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string amendmentid,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var amendment = resolution.Amendments.FirstOrDefault(n => n.ID == amendmentid);
        //    if (amendment == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound, "Amendment not found");
        //    }

        //    amendment.Deactivate();
        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Group(resolutionid).AmendmentDeactivated(new Hubs.HubObjects.HUBResolution(resolution),
        //        new Hubs.HubObjects.HUBAbstractAmendment(amendment));
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Submits the Amendment
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="amendmentid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPatch]
        //public IActionResult SubmitAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string amendmentid,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var amendment = resolution.Amendments.FirstOrDefault(n => n.ID == amendmentid);
        //    if (amendment == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound, "Amendment not found");
        //    }

        //    amendment.Submit();
        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Group(resolutionid).AmendmentSubmitted(new Hubs.HubObjects.HUBResolution(resolution));
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Denies the Amendment
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionid"></param>
        ///// <param name="amendmentid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPatch]
        //public IActionResult DenyAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
        //    [FromHeader]string amendmentid,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(resolutionid);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var amendment = resolution.Amendments.FirstOrDefault(n => n.ID == amendmentid);
        //    if (amendment == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound, "Amendment not found");
        //    }

        //    amendment.Deny();
        //    resolutionService.RequestSave(resolution);

        //    _hubContext.Clients.Group(resolutionid).AmendmentDenied(new HUBResolution(resolution), new HUBAbstractAmendment(amendment));
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        //#endregion

        ///// <summary>
        ///// Returns a list of all the resolutions that the user has access to.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpGet]
        //public ActionResult<List<Resolution>> MyResolutions([FromHeader]string auth,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var user = authService.GetUserOfAuth(auth);
        //    var resolutions = resolutionService.GetResolutionsOfUser(user.UserId);
        //    return StatusCode(StatusCodes.Status200OK, resolutions);
        //}

        ///// <summary>
        ///// Gets a specific resolution with all of its content.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="id"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpGet]
        //public ActionResult<ResolutionModel> Get([FromHeader]string auth, [FromHeader]string id,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(id);
        //    //Wer die offizielle id angibt probiert das Dokument zu bearbeiten
        //    if (resolution != null)
        //    {
        //        var user = authService.GetUserOfAuth(auth);
        //        if (!authService.CanUserEditResolution(user, resolution))
        //            return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //        return StatusCode(StatusCodes.Status200OK, new HUBResolution(resolution));
        //    }

        //    //Wenn die Resolution nicht mit dieser id gefunden wurde handelt
        //    //es sich ggf. um eine Public Id
        //    //Schaue nach, ob die Resolution überhaupt im public mode ist
        //    var info = resolutionService.GetResolutionInfoForPublicId(id);
        //    if (info.PublicRead)
        //    {
        //        resolution = resolutionService.GetResolution(info.ResolutionId);
        //    }

        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    return StatusCode(StatusCodes.Status200OK, new HUBResolution(resolution));
        //}

        ///// <summary>
        ///// Puts the user into the signalR Group for this document.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="id"></param>
        ///// <param name="connectionid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpPut]
        //public IActionResult SubscribeToResolution([FromHeader]string auth, [FromHeader]string id,
        //    [FromHeader]string connectionid,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolution = resolutionService.GetResolution(id);
        //    if (resolution != null)
        //    {
        //        var isPublic = resolutionService.GetResolutionDatabaseModel(id);
        //        if (isPublic.PublicRead)
        //        {
        //            _hubContext.Groups.AddToGroupAsync(connectionid, id);
        //            return StatusCode(StatusCodes.Status200OK);
        //        }
        //        else
        //        {
        //            var user = authService.GetUserOfAuth(auth);
        //            if (!authService.CanUserEditResolution(user, resolution))
        //                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");
        //        }
        //        _hubContext.Groups.AddToGroupAsync(connectionid, id);
        //        return StatusCode(StatusCodes.Status200OK);

        //    }

        //    //public Check
        //    var info = resolutionService.GetResolutionInfoForPublicId(id);
        //    if (info.PublicRead)
        //    {
        //        resolution = resolutionService.GetResolution(info.ResolutionId);
        //    }

        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    _hubContext.Groups.AddToGroupAsync(connectionid, id);



        //    return StatusCode(StatusCodes.Status200OK);
        //}

        ///// <summary>
        ///// Checks if the given auth-key is allowed to edit the given resolution
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="id"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpGet]
        //public ActionResult<bool> CanAuthEditResolution([FromHeader]string auth, [FromHeader]string id,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var resolutionWithId = resolutionService.GetResolution(id);
        //    if (resolutionWithId == null)
        //    {
        //        //Offenbar die Public id
        //        var info = resolutionService.GetResolutionInfoForPublicId(id);
        //        if (info == null)
        //            return StatusCode(StatusCodes.Status404NotFound, "Resolution not found!");
        //        if (!string.IsNullOrWhiteSpace(info.ResolutionId))
        //        {
        //            resolutionWithId = resolutionService.GetResolution(info.ResolutionId);
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, "Resolution not found!");
        //        }
        //    }

        //    var user = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(user, resolutionWithId))
        //        return StatusCode(StatusCodes.Status200OK, false);
        //    else
        //        return StatusCode(StatusCodes.Status200OK, true);
        //}

        ///// <summary>
        ///// Returns all finrmations for the resolution with the given id.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="id"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        //[Route("[action]")]
        //[HttpGet]
        //public ActionResult<Resolution> GetResolutionInfos([FromHeader]string auth, [FromHeader]string id,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService)
        //{
        //    var infos = resolutionService.GetResolutionDatabaseModel(id);
        //    if (infos == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound, "Resolution was not found");
        //    }
        //    return StatusCode(StatusCodes.Status200OK, infos);
        //}

        //[Route("[action]")]
        //[HttpPatch]
        //public ActionResult GrandUserRights([FromHeader]string auth,
        //    [FromBody]GrandUserResolutionRights request,
        //    [FromServices]ResolutionService resolutionService,
        //    [FromServices]AuthService authService,
        //    [FromServices]UserService userService)
        //{
        //    var resolution = resolutionService.GetResolution(request.ResolutionId);
        //    if (resolution == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    var me = authService.GetUserOfAuth(auth);
        //    if (!authService.CanUserEditResolution(me, resolution))
        //        return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    var user = userService.GetUserByUsername(request.Username);
        //    if (user == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "User not found");

        //    resolutionService.GrandUserRights(request.ResolutionId, user.Id, request.CanRead, request.CanEdit);
        //    return StatusCode(StatusCodes.Status200OK);
        //}

        //[Route("[action]")]
        //[HttpPatch]
        //public ActionResult LinkResolutionToConference([FromHeader]string auth,
        //    [FromHeader]string resolutionid, [FromHeader]string conferenceid,
        //    [FromServices]ResolutionService resolutionService, [FromServices]AuthService authService,
        //    [FromServices]ConferenceService conferenceService)
        //{
        //    throw new NotImplementedException("To be reworked");
        //    //var resolution = resolutionService.GetResolution(resolutionid);
        //    //if (resolution == null)
        //    //    return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    //if (!authService.CanAuthEditResolution(auth, resolution))
        //    //    return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    //var conference = conferenceService.GetConference(conferenceid);
        //    //if (conference == null)
        //    //    return StatusCode(StatusCodes.Status404NotFound, "Conference not found!");

        //    //resolutionService.LinkResolutionToConference(resolution, conference);
        //    //return StatusCode(StatusCodes.Status200OK);
        //}

        //[Route("[action]")]
        //[HttpPatch]
        //public ActionResult LinkResolutionToCommittee([FromHeader]string auth,
        //    [FromHeader]string resolutionid, [FromHeader]string committeeid,
        //    [FromServices]ResolutionService resolutionService, [FromServices]AuthService authService,
        //    [FromServices]ConferenceService conferenceService)
        //{
        //    throw new NotImplementedException("To be reworked");
        //    //var resolution = resolutionService.GetResolution(resolutionid);
        //    //if (resolution == null)
        //    //    return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

        //    //if (!authService.CanAuthEditResolution(auth, resolution))
        //    //    return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

        //    //var committee = conferenceService.GetCommittee(committeeid);
        //    //if (committee == null)
        //    //    return StatusCode(StatusCodes.Status404NotFound, "Conference not found!");

        //    //resolutionService.LinkResolutionToCommittee(resolution, committee);
        //    //return StatusCode(StatusCodes.Status200OK);
        //}

        //[Route("[action]")]
        //[HttpGet]
        //public ActionResult<List<Resolution>> GetResolutionsOfConference([FromHeader]string conferenceid,
        //    [FromServices]ResolutionService resolutionService)
        //{
        //    throw new NotImplementedException("To be reworked");
        //    // An dieser Stelle wird auf eine Authentifizierung verzichtet, Resolutionen der Konferenz sind für alle
        //    // Sichtbar!
        //    //return resolutionService.GetResolutionsOfConference(conferenceid);
        //}

        //[Route("[action]")]
        //[HttpGet]
        //public ActionResult<List<Resolution>> GetResolutionsOfCommittee(
        //    [FromHeader]string committeeid, [FromServices]ResolutionService resolutionService)
        //{
        //    throw new NotImplementedException("To be reworked");
        //    //return resolutionService.GetResolutionsOfCommittee(committeeid);
        //}
    }
}
