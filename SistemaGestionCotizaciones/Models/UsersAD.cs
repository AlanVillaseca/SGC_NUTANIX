using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models
{
    public class UsersAD
    {
        public DataTable dtUsers;

        public UsersAD()
        {

        }


        public Boolean getFullNameAD()
        {
            this.dtUsers = new DataTable();

            this.dtUsers.Columns.Add("userId");
            this.dtUsers.Columns.Add("fullName");

            /****/
            DataRow dRowUserFName = dtUsers.NewRow();
            dRowUserFName["userId"] = "prb";
            dRowUserFName["fullName"] = "prueba";
            this.dtUsers.Rows.Add(dRowUserFName);
            /*
            string entryPath = ConfigurationManager.AppSettings["entryPath"];
            DirectoryEntry entry = new DirectoryEntry(entryPath + Environment.UserDomainName);
            foreach (DirectoryEntry child in entry.Children)
            {
                if (child.SchemaClassName == "User")
                {
                    if (((int)child.Properties["UserFlags"].Value & 2) != 2)
                    {
                        DataRow dRowUserFName = dtUsers.NewRow();
                        dRowUserFName["userId"] = child.Name;
                        dRowUserFName["fullName"] = child.Properties["FullName"].Value.ToString();
                        this.dtUsers.Rows.Add(dRowUserFName);
                    }
                }


            }
            */
            return true;
        }

        public string getUserFullName(string userName)
        {
            string fullName = "prueba";
            /*
            string entryPath = ConfigurationManager.AppSettings["entryPath"];
            DirectoryEntry entry = new DirectoryEntry(entryPath + Environment.UserDomainName);
            foreach (DirectoryEntry child in entry.Children)
            {
                if (child.SchemaClassName == "User")
                {
                    if (((int)child.Properties["UserFlags"].Value & 2) != 2 && child.Name == userName)
                    {

                        fullName = child.Properties["FullName"].Value.ToString();

                    }
                }

            }*/

            return fullName;
        }

    }
}