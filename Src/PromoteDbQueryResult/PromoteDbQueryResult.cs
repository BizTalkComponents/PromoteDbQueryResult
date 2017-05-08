using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BizTalkComponents.Utils;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using IComponent = Microsoft.BizTalk.Component.Interop.IComponent;
using BizTalkComponents.Utilities.DbQueryUtility;
using BizTalkComponents.Utilities.DbQueryUtility.Repository;

namespace BizTalkComponents.PipelineComponents.PromoteDbQueryResult.PromoteDbQueryResult
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("68222DB0-2B83-11E7-A608-9928429A23CB")]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    public partial class PromoteDbQueryResult : IComponent, IBaseComponent,
                                        IPersistPropertyBag, IComponentUI
    {
        private IDbQueryRepository _dbQueryRepository = null;

        public PromoteDbQueryResult()
        {
            _dbQueryRepository = new SqlDbQueryRepository();
        }

        public PromoteDbQueryResult(IDbQueryRepository dbQueryRepository)
        {
            _dbQueryRepository = dbQueryRepository;
        }

        //Sample property
        private const string ConnectionStringConfigKeyPropertyName = "ConnectionStringConfigKey";
        private const string QueryPropertyName = "Query";
        private const string ContextPropertyToPromotePropertyName = "ContextPropertyToPromote";

        [DisplayName("ConnectionStringConfigKey")]
        [Description("The connectionstring configuration property key")]
        [RequiredRuntime]
        public string ConnectionStringConfigKey { get; set; }

        [DisplayName("Query")]
        [Description("The query to execute")]
        [RequiredRuntime]
        public string Query { get; set; }

        [DisplayName("ContextPropertyToPromote")]
        [Description("The context property to promote the result to.")]
        [RegularExpression(@"^.*#.*$",
         ErrorMessage = "A property path should be formatted as namespace#property.")]
        [RequiredRuntime]
        public string ContextPropertyToPromote { get; set; }

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            string errorMessage;

            if (!Validate(out errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            var helper = new DbQueryHelper(_dbQueryRepository);
            var val = helper.Query(Query, ConnectionStringConfigKey);

            pInMsg.Context.Promote(new ContextProperty(ContextPropertyToPromote), val);

            return pInMsg;
        }

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            ContextPropertyToPromote = PropertyBagHelper.ReadPropertyBag(propertyBag, ContextPropertyToPromotePropertyName, ContextPropertyToPromote);
            Query = PropertyBagHelper.ReadPropertyBag(propertyBag, QueryPropertyName, Query);
            ConnectionStringConfigKey = PropertyBagHelper.ReadPropertyBag(propertyBag, ConnectionStringConfigKeyPropertyName, ConnectionStringConfigKey);
        }

        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            PropertyBagHelper.WritePropertyBag(propertyBag, ContextPropertyToPromotePropertyName, ContextPropertyToPromote);
            PropertyBagHelper.WritePropertyBag(propertyBag, QueryPropertyName, Query);
            PropertyBagHelper.WritePropertyBag(propertyBag, ConnectionStringConfigKeyPropertyName, ConnectionStringConfigKey);
        }
    }
}
