﻿using Abp.Modules;
using Ztgeo.Gis.Winform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZtgeoGISDesktop.Communication;

namespace ZtgeoGISDesktop
{
    [DependsOn(typeof(ZtgeoGisWinformMoudle)
        ,typeof(ZtgeoGISDesktopCommunicationMoudle))]
    public class ZtgeoGISDesktopMoudle : AbpModule
    {
        public override void PreInitialize()
        {

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {

        }
    }
}
