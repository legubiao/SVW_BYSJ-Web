
using BlazorApp1.Data;
using Blazui.Component.Form;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Models
{
    public class AddEngineerForm:ComponentBase
    {
        internal LabelAlign formAlign;
        [Inject]
        Blazui.Component.MessageBox MessageBox { get; set; }

        protected BForm addEngForm;
        protected void Submit()
        {
            if (!addEngForm.IsValid())
            {
                return;
            }

            var engineer = addEngForm.GetValue<Engineer>();
            _ = MessageBox.AlertAsync(engineer.ToString());
            SqlServerCtl.AddEngineer(engineer);
        }

        protected void Reset()
        {
            addEngForm.Reset();
        }
    }
}
