//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 1.1.4322.2032.
// 
namespace DichiarazioniICI.WsStradario 
{
	using System.Diagnostics;
	using System.Configuration;
	using System.Xml.Serialization;
	using System;
	using System.Web.Services.Protocols;
	using System.ComponentModel;
	using System.Web.Services;
	using OggettiComuniStrade;
    
    
	// <remarks/>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Web.Services.WebServiceBindingAttribute(Name="StradarioSoap", Namespace="http://tempuri.org/")]
	public class Stradario : System.Web.Services.Protocols.SoapHttpClientProtocol 
	{
        
		// <remarks/>
		public Stradario() 
		{
			//this.Url = "http://localhost/WSComuniStrade/Stradario.asmx";
			this.Url = ConfigurationManager.AppSettings["UrlServizioStradario"].ToString();
		}
        
		// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetStrade", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public OggettiComuniStrade.OggettoStrada[] GetStrade(OggettiComuniStrade.OggettoStrada objStrada) 
		{
			object[] results = this.Invoke("GetStrade", new object[] {
																		 objStrada});
			return ((OggettiComuniStrade.OggettoStrada[])(results[0]));
		}
        
		        
		// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetTipiStrade", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public OggettiComuniStrade.OggettoTipoStrada[] GetTipiStrade(OggettiComuniStrade.OggettoTipoStrada objTipoStrada) 
		{
			object[] results = this.Invoke("GetTipiStrade", new object[] {
																			 objTipoStrada});
			return ((OggettiComuniStrade.OggettoTipoStrada[])(results[0]));
		}
        
	}
}
