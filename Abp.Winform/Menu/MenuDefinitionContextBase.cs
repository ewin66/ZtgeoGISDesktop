﻿using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Menu
{
    public abstract class MenuDefinitionContextBase :IMenuDefinitionContext
    {
        protected readonly MenuDictionary Menus;

        protected MenuDefinitionContextBase() {
            Menus = new MenuDictionary();
        }

        public WinformMenu CreateMenu(string name,
              string displayName = null,
              string description = null,
              Permission permission = null,
              MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, 
             Dictionary<string, object> properties = null) {
            if (Menus.ContainsKey(name))
            {
                throw new AbpException("There is already a permission with name: " + name);
            } 
            var menu = new WinformMenu(name, displayName, description, permission, multiTenancySides, properties);
            Menus[menu.Name] = menu;
            return menu;
        }
          
        public virtual WinformMenu GetPermissionOrNull(string name)
        {
            return Menus.GetOrDefault(name);
        }

        public virtual void RemovePermission(string name)
        {
            Menus.Remove(name);
        }
    }
}
