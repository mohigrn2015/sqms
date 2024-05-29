﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace SQMS.Utility
{
    public class DropDownListManager
    {
        public static SelectList GetNameTitle(string SelectedValue = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "Mr.", Text = "Mr." });
            list.Add(new SelectListItem { Value = "Mrs.", Text = "Mrs." });

            return new SelectList(list, "Value", "Text", SelectedValue);
        }

        public static SelectList GetUserType(string SelectedValue = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "0", Text = "External User" });
            list.Add(new SelectListItem { Value = "1", Text = "Internal User" });

            return new SelectList(list, "Value", "Text", SelectedValue);
        }
    }
}
