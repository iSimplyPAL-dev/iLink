﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.2032
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization
Imports OggettiComuniStrade
Imports System.Configuration

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 1.1.4322.2032.
'
Namespace WsStradario
    
    '<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="StradarioSoap", [Namespace]:="http://tempuri.org/")>  _
    Public Class Stradario
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        '<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = ConfigurationManager.AppSettings("UrlServizioStradario")
        End Sub
        
        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetStrade", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function GetStrade(ByVal objStrada As OggettoStrada) As OggettoStrada()
            Dim results() As Object = Me.Invoke("GetStrade", New Object() {objStrada})
            Return CType(results(0), OggettoStrada())

        End Function


        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetTipiStrade", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function GetTipiStrade(ByVal objTipoStrada As OggettoTipoStrada) As OggettoTipoStrada()
            Dim results() As Object = Me.Invoke("GetTipiStrade", New Object() {objTipoStrada})
            Return CType(results(0), OggettoTipoStrada())
        End Function


        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HaStradario", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function HaStradario(ByVal CodEnte As String, ByVal DescrizioneEnte As String) As Boolean
            Dim results() As Object = Me.Invoke("HaStradario", New Object() {CodEnte, DescrizioneEnte})
            Return CType(results(0), Boolean)
        End Function

        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetEnti", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function GetEnti(ByVal objEnte As OggettoEnte) As OggettoEnte()
            Dim results() As Object = Me.Invoke("GetEnti", New Object() {objEnte})
            Return CType(results(0), OggettoEnte())
        End Function

    End Class

End Namespace