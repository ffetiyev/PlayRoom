﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,SuperAdmin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
