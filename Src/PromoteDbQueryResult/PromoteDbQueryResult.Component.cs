using System;
using System.Collections;
using System.Linq;
using BizTalkComponents.Utils;

namespace BizTalkComponents.PipelineComponents.PromoteDbQueryResult.PromoteDbQueryResult
{
    public partial class PromoteDbQueryResult
    {
        public string Name { get { return "PromoteDbQueryResult"; } }
        public string Version { get { return "1.0"; } }
        public string Description { get { return "Will promote the result of a database query"; } }

        public void GetClassID(out Guid classID)
        {
            classID = new Guid("682254C2-2B83-11E7-A608-9928429A23CB");
        }

        public void InitNew()
        {

        }

        public IEnumerator Validate(object projectSystem)
        {
            return ValidationHelper.Validate(this, false).ToArray().GetEnumerator();
        }

        public bool Validate(out string errorMessage)
        {
            var errors = ValidationHelper.Validate(this, true).ToArray();

            if (errors.Any())
            {
                errorMessage = string.Join(",", errors);

                return false;
            }

            errorMessage = string.Empty;

            return true;
        }

        public IntPtr Icon { get { return IntPtr.Zero; } }
    }
}
