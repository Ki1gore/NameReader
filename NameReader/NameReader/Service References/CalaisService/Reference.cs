﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NameReader.CalaisService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://clearforest.com/", ConfigurationName="CalaisService.calaisSoap")]
    public interface calaisSoap {
        
        // CODEGEN: Generating message contract since element name licenseID from namespace http://clearforest.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://clearforest.com/Enlighten", ReplyAction="*")]
        NameReader.CalaisService.EnlightenResponse Enlighten(NameReader.CalaisService.EnlightenRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://clearforest.com/Enlighten", ReplyAction="*")]
        System.Threading.Tasks.Task<NameReader.CalaisService.EnlightenResponse> EnlightenAsync(NameReader.CalaisService.EnlightenRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnlightenRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Enlighten", Namespace="http://clearforest.com/", Order=0)]
        public NameReader.CalaisService.EnlightenRequestBody Body;
        
        public EnlightenRequest() {
        }
        
        public EnlightenRequest(NameReader.CalaisService.EnlightenRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://clearforest.com/")]
    public partial class EnlightenRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string licenseID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string content;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string paramsXML;
        
        public EnlightenRequestBody() {
        }
        
        public EnlightenRequestBody(string licenseID, string content, string paramsXML) {
            this.licenseID = licenseID;
            this.content = content;
            this.paramsXML = paramsXML;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnlightenResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EnlightenResponse", Namespace="http://clearforest.com/", Order=0)]
        public NameReader.CalaisService.EnlightenResponseBody Body;
        
        public EnlightenResponse() {
        }
        
        public EnlightenResponse(NameReader.CalaisService.EnlightenResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://clearforest.com/")]
    public partial class EnlightenResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string EnlightenResult;
        
        public EnlightenResponseBody() {
        }
        
        public EnlightenResponseBody(string EnlightenResult) {
            this.EnlightenResult = EnlightenResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface calaisSoapChannel : NameReader.CalaisService.calaisSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class calaisSoapClient : System.ServiceModel.ClientBase<NameReader.CalaisService.calaisSoap>, NameReader.CalaisService.calaisSoap {
        
        public calaisSoapClient() {
        }
        
        public calaisSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public calaisSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public calaisSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public calaisSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        NameReader.CalaisService.EnlightenResponse NameReader.CalaisService.calaisSoap.Enlighten(NameReader.CalaisService.EnlightenRequest request) {
            return base.Channel.Enlighten(request);
        }
        
        public string Enlighten(string licenseID, string content, string paramsXML) {
            NameReader.CalaisService.EnlightenRequest inValue = new NameReader.CalaisService.EnlightenRequest();
            inValue.Body = new NameReader.CalaisService.EnlightenRequestBody();
            inValue.Body.licenseID = licenseID;
            inValue.Body.content = content;
            inValue.Body.paramsXML = paramsXML;
            NameReader.CalaisService.EnlightenResponse retVal = ((NameReader.CalaisService.calaisSoap)(this)).Enlighten(inValue);
            return retVal.Body.EnlightenResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NameReader.CalaisService.EnlightenResponse> NameReader.CalaisService.calaisSoap.EnlightenAsync(NameReader.CalaisService.EnlightenRequest request) {
            return base.Channel.EnlightenAsync(request);
        }
        
        public System.Threading.Tasks.Task<NameReader.CalaisService.EnlightenResponse> EnlightenAsync(string licenseID, string content, string paramsXML) {
            NameReader.CalaisService.EnlightenRequest inValue = new NameReader.CalaisService.EnlightenRequest();
            inValue.Body = new NameReader.CalaisService.EnlightenRequestBody();
            inValue.Body.licenseID = licenseID;
            inValue.Body.content = content;
            inValue.Body.paramsXML = paramsXML;
            return ((NameReader.CalaisService.calaisSoap)(this)).EnlightenAsync(inValue);
        }
    }
}
