'------------------------------------------------------------------------------
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
Imports log4net

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 1.1.4322.2032.
'
Namespace WsStradario

    '<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Web.Services.WebServiceBindingAttribute(Name:="StradarioSoap", [Namespace]:="http://tempuri.org/")>
    Public Class Stradario
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol

        Private Shared Log As ILog = LogManager.GetLogger(GetType(Stradario))

        '<remarks/>
        Public Sub New()
            MyBase.New()
            Me.Url = ConfigurationManager.AppSettings("UrlServizioStradario")
        End Sub

        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetStrade", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>
        Public Function GetStrade(ByVal objStrada As OggettoStrada) As OggettoStrada()
            Try
                Dim results() As Object = Me.Invoke("GetStrade", New Object() {objStrada})
                Return CType(results(0), OggettoStrada())
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Stradario.GetStrade.errore: ", ex)
            End Try
        End Function


        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetTipiStrade", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>
        Public Function GetTipiStrade(ByVal objTipoStrada As OggettoTipoStrada) As OggettoTipoStrada()
            Try
                Dim results() As Object = Me.Invoke("GetTipiStrade", New Object() {objTipoStrada})
                Return CType(results(0), OggettoTipoStrada())
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Stradario.GetTipiStrade.errore: ", ex)
            End Try
        End Function


        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HaStradario", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function HaStradario(ByVal CodEnte As String, ByVal DescrizioneEnte As String) As Boolean
            Try
                Dim results() As Object = Me.Invoke("HaStradario", New Object() {CodEnte, DescrizioneEnte})
                Return CType(results(0), Boolean)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Stradario.HaStradario.errore: ", ex)
            End Try
        End Function

        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetEnti", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function GetEnti(ByVal objEnte As OggettoEnte) As OggettoEnte()
            Try
                Dim results() As Object = Me.Invoke("GetEnti", New Object() {objEnte})
                Return CType(results(0), OggettoEnte())
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Stradario.GetEnti.errore: ", ex)
            End Try
        End Function

    End Class

End Namespace
