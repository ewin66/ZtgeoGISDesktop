﻿using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ztgeo.Gis.Winform.ABPForm
{
    /// <summary>
    /// MainForm 
    /// </summary>
    public interface IMainForm : ISingletonDependency
    { 
        Control MenuContainerControl { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        void StartInitializeComponent();
    }
}