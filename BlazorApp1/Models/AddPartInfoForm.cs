using BlazorApp1.Data;
using Blazui.Component.Form;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Models
{
    public class AddPartInfoForm:ComponentBase
    {
        internal LabelAlign formAlign;
        [Inject]
        Blazui.Component.MessageBox MessageBox { get; set; }

        protected BForm addPartInfoForm;
        protected void Submit()
        {
            if (!addPartInfoForm.IsValid())
            {
                return;
            }

            var sparePart = addPartInfoForm.GetValue<SparePart>();
            
            _ = MessageBox.AlertAsync(sparePart.ToString());
            SqlServerCtl.AddPartInfo(sparePart);
        }


        protected void Reset()
        {
            addPartInfoForm.Reset();
        }
    }
}
