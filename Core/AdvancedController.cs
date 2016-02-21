using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GovEventer.Models;

namespace GovEventer.Core
{

    public abstract class AdvancedController : Controller
    {
        public virtual bool RequiredAdmin { get { return false; } }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.DataUser = DataUser;
            if (RequiredAdmin && (DataUser == null || !DataUser.IsAdmin))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            base.OnActionExecuting(filterContext);
        }

        public DatabaseContext Db { get { return DatabaseFactory.Instance; } }

        private User _dataUser;
        public const string DataUserSessionKey = "DataUser";
        public User DataUser
        {
            get
            {
                if (_dataUser == null) _dataUser = Session[DataUserSessionKey] as User;
                if (_dataUser != null && User.Identity.IsAuthenticated && _dataUser.EMailAddress == User.Identity.Name) return _dataUser;
                if (!User.Identity.IsAuthenticated) return null;
                _dataUser = Db.Users.FirstOrDefault(x => x.EMailAddress == User.Identity.Name);
                Session[DataUserSessionKey] = _dataUser;
                return _dataUser;
            }
            set { Session[DataUserSessionKey] = _dataUser = value; }
        }

        public void AddCreationInfo(BaseEntity entity)
        {
            entity.CreateDateTime = DateTime.Now;
            entity.CreatorUser = DataUser;
            entity.CreatorUserId = DataUser.Id;
        }

        public void AddModificationInfo(BaseEntity entity)
        {
            entity.LastModifyDateTime = DateTime.Now;
            entity.LastModifierByUser = DataUser;
            entity.LastModifierByUserId = DataUser.Id;
        }
    }
}