<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DemoGaraSportelloContribuente.aspx.vb" Inherits="OPENgov.WebForm1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../StyleDemoGaraSportelloContribuente.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        // Load the Visualization API and the corechart package.
        google.charts.load('current', { 'packages': ['corechart'] });

        // Set a callback to run when the Google Visualization API is loaded.
        google.charts.setOnLoadCallback(drawChart);

        // Callback that creates and populates a data table,
        // instantiates the pie chart, passes in the data and
        // draws it.
        function drawChart() {
            // Create the data table.
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Topping');
            data.addColumn('number', 'Slices');
            data.addRows([
          ['2.30 %', 15],
          ['15.60 %', 99],
          ['82.10 %', 522]
        ]);

            // Set chart options
            var options = { 'title': ''
                ,'width': 400
                , 'height': 300
                , pieSliceTextStyle: {
                    color: 'transparent'
                }
                , slices: {
                    0: { color: 'aqua' }
                    , 1: { color: 'red' }
                    , 2: { color: 'lime' }
                }
            };

            // Instantiate and draw our chart, passing in some options.
            var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
            chart.draw(data, options);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="SfondoVisualizza">
    <!-- SITUAZIONE IMU *** ATESSA-->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneApriElaborazioni" Text="      Storico" Width="90px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneClose" Text="    Cessazione" Width="110px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneAttention" Text="      Inagibilità" Width="110px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneSoggetti" Text="       Uso Gratuito" Width="130px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRettificaAvviso" Text="     Dichiara Variazione" Width="160px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneNewInsert" Text="    Dichiara Nuovo" Width="140px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneModifica" Text="      Modifica" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneApri" Text="       Visualizza" Width="110px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:70px"><br /><br /><br /><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Consultazione ICI/IMU</asp:Label>
    </div>
    <div style="width:100%">
        <p>Gentile <b>Mario Rossi</b>&ensp;&ensp;Codice Fiscale: <b>RSSMRA80A01E379N</b></p>
        <p>L’indirizzo a cui il comune le spedisce le informative è: <b>Viale Garibaldi, 86 - 48034 Fusignano (RA)</b></p>
        <table>
            <tr>
                <td class="Input_Label_title">Legenda</td>
                <td style="width:280px"></td>
                <td class="Input_Label_title">Stato</td>
            </tr>
            <tr>
                <td valign="top">
                    <table style="border:2px solid #0066CC;border-radius: 25px;padding:25px">
                        <tr>
                            <td style="background-color:Green;width:25px;"></td>
                            <td>Tutto OK</td>
                        </tr>
                        <tr>
                            <td style="background-color:yellow"></td>
                            <td>Qualche Differenza</td>
                        </tr>
                        <tr>
                            <td style="background-color:red"></td>
                            <td>Non Trovata</td>
                        </tr>
                    </table>
                </td>
                <td></td>
                <td>
                    <input class="Bottone BottoneFlag" style="width:auto;height:110px" />
                </td>
            </tr>
        </table>
        <p class="Input_Label_title">Situazione dichiarata</p>
        <div>
            <p>In questa sezione trova l'elenco degli immobili da lei dichiarati.</p>            
            <table>
                <tr class="CartListHead">
                    <td>Sel.</td>
                    <td>Stato</td>
                    <td>Leg.</td>
                    <td>Via</td>
                    <td>Foglio</td>
                    <td>Numero</td>
                    <td>Sub.</td>
                    <td>Dal</td>
                    <td>Al</td>
                    <td>Cat.</td>
                    <td>Rendita/Valore</td>
                    <td>Pos.</td>
                    <td>Tipo Ut.</td>
                </tr>
                <tr class="CartListItem">
                    <td><input type="checkbox" /></td>
                    <td style="background-color:Green"></td>
                    <td>1</td>
                    <td>VIA A. MODIGLIANI 25</td>
                    <td>50</td>
                    <td>195</td>
                    <td>2</td>
                    <td>01/01/1992</td>
                    <td></td>
                    <td>A/3</td>
                    <td>457,32</td>
                    <td>100,00</td>
                    <td>Abi.Princ.</td>
                </tr>										
                <tr class="CartListItem">
                    <td><input type="checkbox" /></td>
                    <td style="background-color:Yellow"></td>
                    <td>2</td>
                    <td>VIA A. MODIGLIANI 25</td>
                    <td>50</td>
                    <td>318</td>
                    <td>38</td>
                    <td>01/01/1992</td>
                    <td></td>
                    <td>C/2</td>
                    <td>91,56</td>
                    <td>100,00</td>
                    <td>Pert.</td>
                </tr>										
                <tr class="CartListItem">
                    <td><input type="checkbox"></td>
                    <td style="background-color:Green"></td>
                    <td>3</td>
                    <td>VIA DON PRIMO MAZZOLARI 49</td>
                    <td>50</td>
                    <td>346</td>
                    <td>3</td>
                    <td>01/01/1992</td>
                    <td></td>
                    <td>C/2</td>
                    <td>31,76</td>
                    <td>100,00</td>
                    <td>Fabb.</td>
                </tr>										
                <tr class="CartListItem">
                    <td><input type="checkbox" /></td>
                    <td style="background-color:Yellow"></td>
                    <td>4</td>
                    <td>LOCALITÀ CASTELLO-LUSTIGNANO</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>01/01/1992</td>
                    <td>10/09/2014</td>
                    <td></td>
                    <td>4,23</td>
                    <td>25,00</td>
                    <td>Terr.</td>
                </tr>
                <tr>
                    <td colspan="12" align="right" class="Input_Label_bold">
                        Totale Dovuto
                    </td>
                    <td class="Input_Label_bold" align="right">505,00 €</td>
                </tr>										
                <tr>
                    <td colspan="12" align="right" class="Input_Label_bold">
                        Totale Versato
                    </td>
                    <td class="Input_Label_bold" align="right">202,50 €</td>
                </tr>										
            </table>
        </div>
        <br />
        <p class="Input_Label_title">Situazione accertata</p>        
        <div>
            <p>In questa sezione trova l'elenco degli immobili risultanti da Catasto e altre fonti ufficiali.</p>
            <table>
                <tr class="CartListHead">
                    <td>Sel.</td>
                    <td>Stato</td>
                    <td>Leg.</td>
                    <td>Via</td>
                    <td>Foglio</td>
                    <td>Numero</td>
                    <td>Sub.</td>
                    <td>Dal</td>
                    <td>Al</td>
                    <td>Cat.</td>
                    <td>Rendita/Valore</td>
                    <td>Pos.</td>
                    <td>Tipo Ut.</td>
                </tr>
                <tr class="CartListItem">
                    <td><input type="checkbox"></td>
                    <td style="background-color:Green"></td>
                    <td>1</td>
                    <td>VIA A. MODIGLIANI 25</td>
                    <td>50</td>
                    <td>195</td>
                    <td>2</td>
                    <td>01/01/1992</td>
                    <td></td>
                    <td>A/3</td>
                    <td>457,32</td>
                    <td>100,00</td>
                    <td>Abi.Princ.</td>
                </tr>										
                <tr class="CartListItem">
                    <td><input type="checkbox"></td>
                    <td style="background-color:Yellow"></td>
                    <td>2</td>
                    <td>VIA A. MODIGLIANI 25</td>
                    <td>50</td>
                    <td>318</td>
                    <td>38</td>
                    <td>01/01/1992</td>
                    <td></td>
                    <td>C/6</td>
                    <td>82,12</td>
                    <td>100,00</td>
                    <td>Pert.</td>
                </tr>										
                <tr class="CartListItem">
                    <td><input type="checkbox"></td>
                    <td style="background-color:Green"></td>
                    <td>3</td>
                    <td>VIA DON PRIMO MAZZOLARI 49</td>
                    <td>50</td>
                    <td>346</td>
                    <td>3</td>
                    <td>01/01/1992</td>
                    <td></td>
                    <td>C/2</td>
                    <td>31,76</td>
                    <td>100,00</td>
                    <td>Fabb.</td>
                </tr>										
                <tr class="CartListItem">
                    <td><input type="checkbox"></td>
                    <td style="background-color:Yellow"></td>
                    <td>4</td>
                    <td>LOCALITÀ LUSTIGNANO</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>01/01/1992</td>
                    <td>10/09/2014</td>
                    <td></td>
                    <td>4,23</td>
                    <td>33,33</td>
                    <td>Terr.</td>
                </tr>										
                <tr class="CartListItem">
                    <td><input type="checkbox"></td>
                    <td style="background-color:Red"></td>
                    <td>5</td>
                    <td>LOCALITÀ CASTELLO</td>
                    <td>75</td>
                    <td>65</td>
                    <td>1</td>
                    <td>01/01/1999</td>
                    <td></td>
                    <td>A/10</td>
                    <td>808,19</td>
                    <td>50,00</td>
                    <td>Fabb.</td>
                </tr>										
            </table>
        </div>
    </div>-->
    <!-- SITUAZIONE TARI *** MERCATO SARACENO-->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottonePay" Text="      F24" Width="70px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneClose" Text="    Cessazione" Width="110px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneAttention" Text="      Inagibilità" Width="110px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRettificaAvviso" Text="     Dichiara Variazione" Width="160px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneNewInsert" Text="    Dichiara Nuovo" Width="140px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneModifica" Text="      Modifica" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneApri" Text="       Visualizza" Width="110px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:70px"><br /><br /><br /><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Consultazione TARI</asp:Label>
    </div>
    <div style="width:100%">
        <p>Gentile <b>Mario Rossi</b>&ensp;&ensp;Codice Fiscale: <b>RSSMRA80A01E379N</b></p>
        <p>L’indirizzo a cui il comune le spedisce le informative è: <b>Viale Garibaldi, 86 - 48034 Fusignano (RA)</b></p>
        <table>
            <tr>
                <td class="Input_Label_title">Legenda</td>
                <td style="width:280px"></td>
                <td class="Input_Label_title">Stato</td>
            </tr>
            <tr>
                <td valign="top">
                    <table style="border:2px solid #0066CC;border-radius: 25px;padding:25px">
                        <tr>
                            <td style="background-color:Green;width:25px;"></td>
                            <td>Tutto OK</td>
                        </tr>
                        <tr>
                            <td style="background-color:yellow"></td>
                            <td>Qualche Differenza</td>
                        </tr>
                        <tr>
                            <td style="background-color:red"></td>
                            <td>Non Trovata</td>
                        </tr>
                    </table>
                </td>
                <td></td>
                <td>
                    <input class="Bottone BottoneFlag" style="width:auto;height:110px" />
                </td>
            </tr>
        </table>
        <p class="Input_Label_title">Situazione dichiarata</p>
        <div>
            <p>In questa sezione trova l'elenco degli immobili da Lei dichiarati.</p>            
            <table>
                <tr class="CartListHead">
                    <td>Sel.</td>
                    <td>Stato</td>
                    <td>Via</td>
                    <td>Foglio</td>
                    <td>Numero</td>
                    <td>Sub.</td>
                    <td>Dal</td>
                    <td>Al</td>
                    <td>Cat. *</td>
                    <td>MQ</td>
                    <td>MQ Catasto</td>
                    <td>Rid./Esen. **</td>
                </tr>
                <tr class="CartListItem">
                    <td><input type="checkbox" /></td>
                    <td style="background-color:Green"></td>
                    <td>VIA A. MODIGLIANI 25</td>
                    <td>50</td>
                    <td>195</td>
                    <td>2</td>
                    <td>01/01/1992</td>
                    <td></td>
                    <td>DOM 4 NC</td>
                    <td>124</td>
                    <td>124</td>
                    <td></td>
                </tr>										
                <tr class="CartListItem">
                    <td><input type="checkbox" /></td>
                    <td style="background-color:Yellow"></td>
                    <td>VIA A. MODIGLIANI 25</td>
                    <td>50</td>
                    <td>318</td>
                    <td>38</td>
                    <td>01/01/1992</td>
                    <td></td>
                    <td>DOM 0 NC</td>
                    <td>10</td>
                    <td>12</td>
                    <td></td>
                </tr>										
                <tr class="CartListItem">
                    <td><input type="checkbox" /></td>
                    <td style="background-color:Yellow"></td>
                    <td>VIA A. MODIGLIANI 25</td>
                    <td>50</td>
                    <td>318</td>
                    <td>39</td>
                    <td>01/01/1992</td>
                    <td></td>
                    <td>DOM 0 NC</td>
                    <td>9</td>
                    <td>7</td>
                    <td>R02</td>
                </tr>										
                <tr class="CartListItem">
                    <td><input type="checkbox"></td>
                    <td style="background-color:Green"></td>
                    <td>VIA DON PRIMO MAZZOLARI 49</td>
                    <td>50</td>
                    <td>346</td>
                    <td>3</td>
                    <td>01/01/1992</td>
                    <td></td>
                    <td>21</td>
                    <td>98</td>
                    <td>98</td>
                    <td>R01</td>
                </tr>										
                <tr class="CartListItem">
                    <td><input type="checkbox" /></td>
                    <td style="background-color:Red"></td>
                    <td>LOCALITÀ CASTELLO-LUSTIGNANO</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>01/01/1992</td>
                    <td>10/09/2015</td>
                    <td>26</td>
                    <td>75</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td><br /><br /></td>
                </tr>
                <tr>
                    <td valign="top">
                    *
                    </td>
                    <td colspan="9">
                    21 - ATTIVITA' ARTIGIANALI DI PRODUZIONE BENI SPECIFICI<br />
                    26 - PLURILICENZE ALIMENTARI E/O MISTE
                    </td>
                </tr>	
                <tr>
                    <td colspan="12"><hr /></td>
                </tr>
                <tr>
                    <td valign="top">
                    **
                    </td>
                    <td colspan="9">
                    R01 - RIDUZIONE 10% COMPOSTAGGIO CERTIFICATO<br />
                    R02 - IMMOBILE ESENTE
                    </td>
                </tr>									
            </table>
        </div>
        <br /><br />
        <p class="Input_Label_title">Situazione dovuto</p>
        <div>
            <table>
                <tr class="CartListHead">
                    <td>Anno</td>
                    <td>Fissa €</td>
                    <td>Variabile €</td>
                    <td>Imposta €</td>
                    <td>Provinciale €</td>
                    <td>Arrotondamento €</td>
                    <td>Dovuto €</td>
                    <td>Versato €</td>
                </tr>
                <tr class="CartListItem">
                    <td>2016</td>
                    <td align="right">234,31</td>
                    <td align="right">175,90</td>
                    <td align="right">410,21</td>
                    <td align="right">20,51</td>
                    <td align="right">0,28</td>
                    <td align="right">431</td>
                    <td align="right">215,50</td>
                </tr>										
            </table>
        </div>
    </div>-->
    <!-- VARIAZIONE IMU-->
    <!--<div style="width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;        
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneSalva" Text="Salva" Width="105px"/>        
        <asp:Button runat="server" Cssclass="Bottone BottoneAnnulla" Text="Annulla" Width="110px"/>
    </div>
    <div class="SfondoGenerale" style="height:40px"><br /><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Variazione IMU</asp:Label>
    </div>
    <div style="width:100%">
        <p>Gentile <b>Mario Rossi</b>&ensp;&ensp;Codice Fiscale: <b>RSSMRA80A01E379N</b></p>
        <p>L’indirizzo a cui il comune le spedisce le informative è: <b>Viale Garibaldi, 86 - 48034 Fusignano (RA)</b></p>
        <p>In questa sezione può modificare:</p>
        <p class="Input_Label_title">Dati Immobile</p>
        <table style="border:2px solid #0066CC;border-radius: 20px;padding:25px;width:700px">
            <tr>
                <td colspan="2">Ubicazione - Via:</td>
                <td>Civico:</td>
                <td>Caratteristica</td>
            </tr>
            <tr>
                <td class="Input_Label_Italic" colspan="2">Via Garibaldi</td>
                <td class="Input_Label_Italic">86</td>
                <td class="Input_Label_Italic">Fabbricato</td>
            </tr>
            <tr>
                <td colspan="2"><asp:TextBox runat="server" CssClass="Input_Text" Width="300px">Viale Garibaldi</asp:TextBox></td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="50px">86</asp:TextBox></td>
                <td>
                    <select name="categoria" class="Input_Text">
                        <option value="o" selected="selected">Fabbricato</option>
                        <option value="no"></option>
                    </select>
                </td>
            </tr>
                <tr>
                    <td>Data Inizio Possesso:</td>
                    <td colspan="2">Data Fine Possesso:</td>                    
                </tr>
            <tr>
                <td class="Input_Label_Italic">12/06/2000</td>
                <td class="Input_Label_Italic" colspan="2">12/11/2014</td>
            </tr>
                <tr>
                    <td><asp:TextBox runat="server" CssClass="Input_Text_Right" Width="80px">12/06/2000</asp:TextBox></td>
                    <td><asp:TextBox runat="server" CssClass="Input_Text_Right" Width="80px">12/11/2014</asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Dati Catastali - Foglio:</td>
                    <td>Numero:</td>
                    <td>Subalterno:</td>
                    <td>Zona:</td>
                </tr>
            <tr>
                <td class="Input_Label_Italic">173</td>
                <td class="Input_Label_Italic">16</td>
                <td class="Input_Label_Italic">1</td>
                <td class="Input_Label_Italic">1</td>
            </tr>
                <tr>
                    <td><asp:TextBox runat="server" CssClass="Input_Text" Width="50px">173</asp:TextBox></td>
                    <td><asp:TextBox runat="server" CssClass="Input_Text" Width="50px">16</asp:TextBox></td>
                    <td><asp:TextBox runat="server" CssClass="Input_Text" Width="50px">1</asp:TextBox></td>
                    <td><asp:TextBox runat="server" CssClass="Input_Text" Width="50px">1</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Categoria Catastale:</td>
                    <td>Classe:</td>
                    <td>Consistenza:</td>
                    <td>Rendita:<asp:Button runat="server" Cssclass="Bottone BottoneHelpMini" Text=""/></td>
                     <td>Storico:</td>
               </tr>
            <tr>
                <td class="Input_Label_Italic">A/2</td>
                <td class="Input_Label_Italic">1</td>
                <td class="Input_Label_Italic">5</td>
                <td class="Input_Label_Italic">356,90</td>
            </tr>
                <tr>
                    <td><asp:TextBox runat="server" CssClass="Input_Text">A/2</asp:TextBox></td>
                    <td><asp:TextBox runat="server" CssClass="Input_Text" Width="50px">1</asp:TextBox></td>
                    <td><asp:TextBox runat="server" CssClass="Input_Text_Right" Width="50px">5</asp:TextBox></td>
                    <td><asp:TextBox runat="server" CssClass="Input_Text_Right" Width="100px">430,10</asp:TextBox></td>
                    <td>
                        <input type="radio" name="d" value="si">Si</input>
                        <input type="radio" name="d" value="no">No</input>
                    </td>
                </tr>
                <tr>
                </tr>
            </table>
        <p class="Input_Label_title">&nbsp;&nbsp;Dati Per Calcolo</p>
            <table style="border:2px solid #0066CC;border-radius: 20px;padding:25px;width:700px">
                <tr>
                    <td>Tipo di Utilizzo: <asp:Button runat="server" Cssclass="Bottone BottoneHelpMini" Text=""/></td>
                    <td >Numero Utilizzatori: <asp:Button runat="server" Cssclass="Bottone BottoneHelpMini" Text=""/></td>
                    <td>Quota Possesso</td>
                    <td>Coltivatore diretto</td>
                </tr>
            <tr>
                <td class="Input_Label_Italic">Abitazione Principale</td>
                <td class="Input_Label_Italic">2</td>
                <td class="Input_Label_Italic">50</td>
                <td class="Input_Label_Italic">No</td>
            </tr>
                <tr>
                    <td>
                        <select name="categoria" class="Input_Text">
                                       <option value="cat1" selected="selected">Abitazione Principale</option>
                                       <option value="cat1"></option>
                        </select>
                    </td>
                    <td style="vertical-align:top;"><asp:TextBox runat="server" CssClass="Input_Text" Width="50px">2</asp:TextBox></td>                    
                    <td><asp:TextBox runat="server" CssClass="Input_Text_Right" Width="50px">100</asp:TextBox></td>
                    <td><input type="checkbox" /></td>
                </tr>
                <tr>
                    <td>Pertinenza di:</td>
                </tr>
                <tr>
                    <td>Numero:<asp:TextBox runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox></td>
                    <td>Subalterno:<asp:TextBox runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox></td>
                    <td>Zona:<asp:TextBox runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox></td>
                </tr>
            <tr>
                <td class="Input_Label_Italic"></td>
                <td class="Input_Label_Italic"></td>
                <td class="Input_Label_Italic"></td>
                <td class="Input_Label_Italic"></td>
            </tr>
                <tr>
                    <td><input type="checkbox">Esenzione</td>
                    <td><input type="checkbox">Riduzione</td>
                </tr>
            </table>
            <hr />
            <div style="height:20px;">
                <asp:CheckBox runat="server" CssClass="Input_Label_bold" Text="Allego Documentazione" />
            </div>
            <br />              
    </div>-->
    <!-- VARIAZIONE TARI -->
    <!--<div style="width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneSalva" Text="Salva" Width="105px"/>        
        <asp:Button runat="server" Cssclass="Bottone BottoneAnnulla" Text="Annulla" Width="110px"/>
    </div>
    <div class="SfondoGenerale" style="height:30px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Variazione TARI</asp:Label>
    </div><br />
    <div style="width:100%">
        <p>Gentile <b>Mario Rossi</b>&ensp;&ensp;Codice Fiscale: <b>RSSMRA80A01E379N</b></p>
        <p>L’indirizzo a cui il comune le spedisce le informative è: <b>Viale Garibaldi, 86 - 48034 Fusignano (RA)</b></p>
        <p>In questa sezione può modificare:</p>
        <p class="Input_Label_title">Dati Immobile</p>
        <table style="border:2px solid #0066CC;border-radius: 20px;padding:25px;width:700px">
            <tr>
                <td colspan="2">Ubicazione - Via:</td>
                <td>Civico:</td>
                <td>Stato Occupazione:<asp:Button runat="server" Cssclass="Bottone BottoneHelpMini" Text=""/></td>
            </tr>
            <tr>
                <td class="Input_Label_Italic" colspan="2">Via Garibaldi</td>
                <td class="Input_Label_Italic">86</td>
                <td class="Input_Label_Italic">Occupato</td>
            </tr>
            <tr>
                <td colspan="2"><asp:TextBox runat="server" CssClass="Input_Text" Width="300px">Viale Garibaldi</asp:TextBox></td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="50px">86</asp:TextBox></td>
                    <td>
                        <select name="categoria" class="Input_Text">
                            <option value="o" selected="selected">Occupato</option>
                            <option value="no">Non Occupato</option>
                        </select>
                   </td>
                </tr>
                <tr>
                    <td>Data Inizio Occupazione:</td>
                    <td>Data Fine Occupazione:</td>
                    <td></td>
                </tr>
            <tr>
                <td class="Input_Label_Italic">12/06/2000</td>
                <td class="Input_Label_Italic" colspan="2">12/11/2014</td>
            </tr>
                <tr>
                    <td><asp:TextBox runat="server" CssClass="Input_Text_Right" Width="80px">12/06/2000</asp:TextBox></td>
                    <td><asp:TextBox runat="server" CssClass="Input_Text_Right" Width="80px">12/11/2014</asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Dati Catastali - Foglio:</td>
                    <td>Numero:</td>
                    <td>Subalterno:</td>
                </tr>
            <tr>
                <td class="Input_Label_Italic">173</td>
                <td class="Input_Label_Italic">16</td>
                <td class="Input_Label_Italic">1</td>
            </tr>
                <tr>
                    <td><asp:TextBox runat="server" CssClass="Input_Text" Width="50px">173</asp:TextBox></td>
                    <td><asp:TextBox runat="server" CssClass="Input_Text" Width="50px">16</asp:TextBox></td>
                    <td><asp:TextBox runat="server" CssClass="Input_Text" Width="50px">1</asp:TextBox></td>
                </tr>
            </table>
        <p class="Input_Label_title">Dati Per Calcolo</p>
        <table style="border:2px solid #0066CC;border-radius: 20px;padding:25px;width:700px">
            <tr>
                <td>Tipo di Utilizzo:<asp:Button runat="server" Cssclass="Bottone BottoneHelpMini" Text=""/></td>
                <td class="Input_Label_Italic">Domestico</td>
                <td><asp:RadioButton runat="server" CssClass="Input_Radio" GroupName="a" Text="Domestico" Checked="true" /></td>
                <td><asp:RadioButton runat="server" CssClass="Input_Radio" GroupName="a" Text="Domestico Pertinenziale" /></td>
                <td><asp:RadioButton runat="server" CssClass="Input_Radio" GroupName="a" Text="Non Domestico" /></td>
            </tr>
                <tr>
                <td >Numero Componenti:</td>
                <td colspan="3">Attività:</td>
                <td >MQ:<asp:Button runat="server" Cssclass="Bottone BottoneHelpMini" Text=""/></td>
            </tr>
        <tr>
            <td class="Input_Label_Italic">4</td>
            <td class="Input_Label_Italic" colspan="3"></td>
            <td class="Input_Label_Italic">160</td>
        </tr>
            <tr>
                <td ><asp:TextBox runat="server" CssClass="Input_Text" width="50px">3</asp:TextBox></td>
                <td colspan="3">
                    <select name="categoria" class="Input_Text" disabled="disabled" style="background-color:lightgray;width:350px">
                                    <option value="cat1" selected="selected">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</option>
                                    <option value="cat1"></option>
                    </select>
                </td>
                <td ><asp:TextBox runat="server" CssClass="Input_Text" width="50px">160</asp:TextBox></td>                    
            </tr>
            <tr>
                <td>Esenzioni:</td>
                <td>Riduzioni:</td>
            </tr>
        <tr>
            <td class="Input_Label_Italic"></td>
            <td class="Input_Label_Italic">Compostaggio</td>
        </tr>
            <tr>
                <td>
                    <select name="categoria" class="Input_Text">
                        <option value="o" selected="selected"></option>
                        <option value="no">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</option>
                    </select>
                </td>
                <td>
                    <select name="categoria" class="Input_Text">
                        <option value="o" selected="selected">Compostaggio</option>
                        <option value="no">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</option>
                    </select>
                </td>
            </tr>
        </table>
        <p class="Input_Label_title">Motivazione</p>
        <table style="border:2px solid #0066CC;border-radius: 20px;padding:25px;width:700px">
            <tr>
                <td><asp:TextBox runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox></td>
            </tr>
            <tr><td><hr /></td></tr>
            <tr><td><asp:CheckBox runat="server" CssClass="Input_Label_bold" Text="Allego Documentazione" /></td></tr>
        </table>
    </div>-->
    <!-- SITUAZIONE CONTRIBUENTE -->
    <!--<div style="width:100%">
        <p>Gentile <b>Mario Rossi</b>&ensp;&ensp;Codice Fiscale: <b>RSSMRA80A01E379N</b></p>
        <p>L’indirizzo a cui il comune le spedisce le informative è: <b>Viale Garibaldi, 86 - 48034 Fusignano (RA)</b></p>
        <p class="Input_Label_title">Situazione Dichiarata IMU</p>
        <table>
            <tr class="CartListHead">
                <td>Via</td>
                <td>Foglio</td>
                <td>Numero</td>
                <td>Sub.</td>
                <td>Dal</td>
                <td>Al</td>
                <td>Cat.</td>
                <td>Rendita/Valore</td>
                <td>Pos.</td>
                <td>Tipo Ut.</td>
            </tr>
            <tr class="CartListItem">
                <td>VIA A. MODIGLIANI 25</td>
                <td>50</td>
                <td>195</td>
                <td>2</td>
                <td>01/01/1992</td>
                <td></td>
                <td>A/3</td>
                <td>457,32</td>
                <td>100,00</td>
                <td>Abi.Princ.</td>
            </tr>										
            <tr class="CartListItem">
                <td>VIA A. MODIGLIANI 25</td>
                <td>50</td>
                <td>318</td>
                <td>38</td>
                <td>01/01/1992</td>
                <td></td>
                <td>C/2</td>
                <td>91,56</td>
                <td>100,00</td>
                <td>Pert.</td>
            </tr>										
            <tr class="CartListItem">
                <td>VIA DON PRIMO MAZZOLARI 49</td>
                <td>50</td>
                <td>346</td>
                <td>3</td>
                <td>01/01/1992</td>
                <td></td>
                <td>C/2</td>
                <td>31,76</td>
                <td>100,00</td>
                <td>Fabb.</td>
            </tr>										
            <tr class="CartListItem">
                <td>LOCALITÀ CASTELLO-LUSTIGNANO</td>
                <td></td>
                <td></td>
                <td></td>
                <td>01/01/1992</td>
                <td>10/09/2014</td>
                <td></td>
                <td>4,23</td>
                <td>25,00</td>
                <td>Terr.</td>
            </tr>
            <tr class="CartListItem">
                <td>VIA COLOMBO 96</td>
                <td>23</td>
                <td>101</td>
                <td>2</td>
                <td>01/03/1996</td>
                <td>28/08/2002</td>
                <td>A/2</td>
                <td>103,45</td>
                <td>100,00</td>
                <td>Fabb.</td>
            </tr>										
        </table>
        <p class="Input_Label_title">Situazione Dovuto IMU</p>
        <table>
            <tr class="CartListHead">
                <td>Anno</td>
                <td>Abi. Princ.</td>
                <td>Altri Fab.</td>
                <td>Aree Fab.</td>
                <td>Terreni</td>
                <td>Fab.Rur.</td>
                <td>Uso Prod.Cat.D</td>
                <td>Detraz.</td>
                <td>Totale</td>
                <td>Num. Fab.</td>
            </tr>
            <tr class="CartListItem">
                <td>2015</td>
                <td>0,00</td>
                <td>25,35</td>
                <td>0,00</td>
                <td>0,00</td>
                <td>0,00</td>
                <td>0,00</td>
                <td>0,00</td>
                <td>25,35</td>
                <td>3</td>
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Pagato IMU</p>
        <table>
            <tr class="CartListHead">
                <td>Anno</td>
                <td>Data</td>
                <td>Tipo</td>
                <td>Totale</td>
            </tr>
            <tr class="CartListItem">
                <td>2015</td>
                <td>05/06/2015</td>
                <td>Unica soluzione</td>
                <td>25,35</td>
            </tr>
        </table>
        <br />
        <p class="Input_Label_title">Situazione Dichiarata TARI</p>
        <table>
            <tr class="CartListHead">
                <td>Via</td>
                <td>Foglio</td>
                <td>Numero</td>
                <td>Sub.</td>
                <td>Dal</td>
                <td style="width:80px">Al</td>
                <td>Categoria</td>
                <td>Componenti</td>
                <td>MQ</td>
            </tr>
            <tr class="CartListItem">
                <td>VIA A. MODIGLIANI 25</td>
                <td>50</td>
                <td>195</td>
                <td>2</td>
                <td>01/01/1992</td>
                <td></td>
                <td>Domestica</td>
                <td>2</td>
                <td>89</td>
            </tr>										
            <tr class="CartListItem">
                <td>VIA A. MODIGLIANI 25</td>
                <td>50</td>
                <td>318</td>
                <td>38</td>
                <td>01/01/1992</td>
                <td></td>
                <td>Pertinenziale</td>
                <td>2</td>
                <td>14</td>
            </tr>										
        </table>
        <p class="Input_Label_title">Situazione Dovuto TARI</p>
        <table>
            <tr class="CartListHead">
                <td>N.Avviso</td>
                <td>Imposta Fissa</td>
                <td>Imposta Variabile</td>
                <td>Provinciale</td>
                <td>Totale</td>
            </tr>
            <tr class="CartListItem">
                <td>1251</td>
                <td>68,85</td>
                <td>63,94</td>
                <td>0,00</td>
                <td>139,00</td>
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Pagato TARI</p>
        <table>
            <tr class="CartListHead">
                <td>N.Avviso</td>
                <td>Data</td>
                <td>Tipo</td>
                <td>Importo</td>
            </tr>
            <tr class="CartListItem">
                <td>1251</td>
                <td>23/07/2015</td>
                <td>F24</td>
                <td>69,50</td>                
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Dicharata OSAP</p>
        <table>
            <tr class="CartListHead">
                <td>Via</td>
                <td>Dal</td>
                <td>Durata</td>
                <td>Tipologia Occupazione</td>
                <td>Consistenza</td>
            </tr>
            <tr class="CartListItem">
                <td>Via Colombo 42</td>
                <td>23/07/2015</td>
                <td>100 giorni</td>
                <td>Occupazione con tende parasole, faretti, vetrinette</td>                
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Dovuto OSAP</p>
        <table>
            <tr class="CartListHead">
                <td>N.Avviso</td>
                <td>Totale</td>
            </tr>
            <tr class="CartListItem">
                <td>5208</td>
                <td>30,00</td>
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Pagato OSAP</p>
        <table>
            <tr class="CartListHead">
                <td>N.Avviso</td>
                <td>Data</td>
                <td>Tipo</td>
                <td>Importo</td>
            </tr>
            <tr class="CartListItem">
                <td></td>
                <td></td>
                <td></td>
                <td></td>                
            </tr>
        </table>
    </div>-->
    <!-- VIDEATA PAGATO -->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="     Stampa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br /><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Estrazione Pagamenti</asp:Label>
    </div><br />
    <div style="width:100%">
        <p>Gentile <b>Mario Rossi</b>&ensp;&ensp;Codice Fiscale: <b>RSSMRA80A01E379N</b></p>
        <p>L’indirizzo a cui il comune le spedisce le informative è: <b>Viale Garibaldi, 86 - 48034 Fusignano (RA)</b></p>
        <asp:Label runat="server">Pagamenti alla data</asp:Label>&nbsp;<asp:TextBox runat="server" CssClass="Input_Text_Right" Width="80px"></asp:TextBox><br />
        <p class="Input_Label_title">Situazione Pagato IMU</p>
        <table>
            <tr class="CartListHead">
                <td>Anno</td>
                <td>Data</td>
                <td>Tipo</td>
                <td>Totale</td>
            </tr>
            <tr class="CartListItem">
                <td>2015</td>
                <td>05/06/2015</td>
                <td>Unica soluzione</td>
                <td>25,35</td>
            </tr>
        </table>
        <br />
        <p class="Input_Label_title">Situazione Pagato TARI</p>
        <table>
            <tr class="CartListHead">
                <td>N.Avviso</td>
                <td>Data</td>
                <td>Tipo</td>
                <td>Importo</td>
            </tr>
            <tr class="CartListItem">
                <td>1251</td>
                <td>23/07/2015</td>
                <td>F24</td>
                <td>69,50</td>                
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Pagato OSAP</p>
        <table>
            <tr class="CartListHead">
                <td>N.Avviso</td>
                <td>Data</td>
                <td>Tipo</td>
                <td>Importo</td>
            </tr>
            <tr class="CartListItem">
                <td></td>
                <td></td>
                <td></td>
                <td></td>                
            </tr>
        </table>
    </div>-->
    <!-- VIDEATA INSOLUTI -->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="     Stampa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br /><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Situazione Insoluti</asp:Label>
    </div><br />
    <div style="width:100%">
        <p>Gentile <b>Mario Rossi</b>&ensp;&ensp;Codice Fiscale: <b>RSSMRA80A01E379N</b></p>
        <p>L’indirizzo a cui il comune le spedisce le informative è: <b>Viale Garibaldi, 86 - 48034 Fusignano (RA)</b></p>
        <p class="Input_Label_title">Situazione Dovuto TARI</p>
        <table>
            <tr class="CartListHead">
                <td>Anno</td>
                <td>N.Avviso</td>
                <td>Dovuto</td>
                <td>Pagato</td>
                <td>Insoluto</td>
            </tr>
            <tr class="CartListItem">
				<td>2015</td>
                <td>1251</td>
                <td>139,00</td>
                <td>69,50</td>
                <td>69,50</td>
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Dovuto OSAP</p>
        <table>
            <tr class="CartListHead">
                <td>Anno</td>
                <td>N.Avviso</td>
                <td>Dovuto</td>
                <td>Pagato</td>
                <td>Insoluto</td>
            </tr>
            <tr class="CartListItem">
				<td>2015</td>
                <td>5208</td>
                <td>30,00</td>
                <td>00,00</td>
                <td>30,00</td>
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Accertamenti</p>
        <table>
            <tr class="CartListHead">
                <td>Anno</td>
                <td>Avviso</td>
                <td>Dovuto</td>
                <td>Pagato</td>
                <td>Insoluto</td>
            </tr>
            <tr class="CartListItem">
				<td>2015</td>
                <td>Avviso di rettifica TARSU</td>
                <td>99,00</td>
                <td>33,00</td>
                <td>66,00</td>
            </tr>
        </table>
    </div>-->
    <!-- VIDEATA INIZIALE *** ATESSA -->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br /><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Cartella Unica del Contribuente</asp:Label>
    </div><br />
    <div style="width:100%">
        <table width="100%">
            <tr>
                <td colspan="2"></td>
                <td>Benvenuto/a <b>Mario Rossi</b></td>
            </tr>
            <tr valign="top">
                <td class="Input_Label_title" style="width:150px">
                    <table>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />Gestione Profilo</td>
                        </tr>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />Reports</td>
                        </tr>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />Paga</td>
                        </tr>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />F.A.Q.</td>
                        </tr>
                    </table>
                </td>
                <td>&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;</td>
                <td>Da questa pagina è possibile accedere alle schede informative sui tributi di pertinenza dell'Amministrazione comunale.<br />
                Si tratta, nel dettaglio:<br />
                    <table>
                        <tr>
                            <td>&ensp;</td>
                            <td>dell'Imposta Municipale Propria <u>(IMU)</u>;</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                        <tr>
                            <td>&ensp;</td>
                            <td>della Tassa sui rifiuti <u>(TARI)</u>;</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                        <tr>
                            <td>&ensp;</td>
                            <td>della Tassa sull'occupazione suolo pubblico <u>(OSAP)</u>;</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                        <tr>
                            <td>&ensp;</td>
                            <td>dell'Imposta sulla Pubblicità <u>(ICP)</u>;</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                    </table>
		            il sito permette la consultazione e la modifica della propria situazione tributaria nonché del pagamento degli importi dovuti per OSAP e ICP.
                    <br /><br /><br /><br /><br /><br />
                    <table>
                        <tr>
                            <td style="width:200px">Situazione debito/credito verso il comune:</td>
                            <td style="width:200px">Totale Dovuto <b>846,00 €</b><br />
                                Totale Pagato <b>592,20 €</b><br />
                                Totale Insoluto <b>253,80 €</b><br />
                            </td>                            
                            <td><img alt="" src="../images/Bottoni/gara_3d-charts-and-pies-vector4.jpg" width="10%" /></td>
                        </tr>
                    </table>
                    <br /><br /><br /><br /><br /><br />
                    <table>
                        <tr>
                            <td colspan="4" class="Input_Label_bold">Stato Istanze aperte verso il Comune <asp:Button runat="server" Cssclass="Bottone BottoneHelpMini" Text=""/></td>
                        </tr>
                        <tr class="CartListHead">
                            <td>Tributo</td>
                            <td>Tipo</td>
                            <td>Data di presentazione</td>
                            <td>Stato</td>
                        </tr>
                        <tr class="CartListItem">
                            <td>IMU</td>
                            <td>Modifica di una posizione accertata</td>
                            <td>26/03/2014</td>
                            <td>Registrata</td>
                        </tr>
                        <tr class="CartListItem">
                            <td>IMU</td>
                            <td>Dichiarazione di Variazione</td>
                            <td>09/07/2015</td>
                            <td>Presa in carico, in attesa di essere registrata</td>
                        </tr>
                        <tr class="CartListItem">
                            <td>TARSU</td>
                            <td>Dichiarazione di Inagibilità</td>
                            <td>16/10/2015</td>
                            <td>In attesa di verifica</td>
                        </tr>
                    </table>
                    <br /><br />
                    E' inoltre possibile consultare le situazioni pregresse per<br />
                    <table>
                        <tr>
                            <td>&ensp;</td>
                            <td>dell’Imposta Comunale sugli Immobili <u>(ICI)</u>;</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                        <tr>
                            <td>&ensp;</td>
                            <td>della Tassa sui rifiuti <u>(TARSU)</u>;</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
		</table>
    </div>-->
    <!-- VIDEATA INIZIALE *** MERCATO SARACENO -->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        &ensp;<asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br /><br />
        <asp:Label runat="server" CssClass="Input_Label_login">&ensp;HOMEPAGE</asp:Label>
    </div><br />
    <div style="width:100%">
        <table width="100%">
            <tr>
                <td colspan="2"></td>
                <td>Benvenuto/a <b>Mario Rossi</b></td>
            </tr>
            <tr valign="top">
                <td class="Input_Label_title" style="width:200px">
                    <table>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />Gestione Profilo</td>
                        </tr>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />Reports</td>
                        </tr>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />Corrispondenza con Ente</td>
                        </tr>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />F.A.Q.</td>
                        </tr>
                    </table>
                </td>
                <td>&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;</td>
                <td>Da questa pagina è possibile accedere alle schede informative sui tributi di pertinenza dell'Amministrazione comunale.<br />
                Si tratta, nel dettaglio:<br />
                    <table>
                        <tr>
                            <td>&ensp;</td>
                            <td>dell'Imposta Municipale Propria <u>(IMU)</u>;</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                        <tr>
                            <td>&ensp;</td>
                            <td>della Tassa sui rifiuti <u>(TARI)</u>;</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                    </table>
		            <br /><br /><br /><br /><br />
                    <table>
                        <tr>
                            <td style="width:200px">Situazione debito/credito verso il comune:</td>
                            <td style="width:200px">Totale Dovuto <b>846,00 €</b><br />
                                Totale Pagato <b>592,20 €</b><br />
                                Totale Insoluto <b>253,80 €</b><br />
                            </td>                            
                            <td><img alt="" src="../images/Bottoni/gara_3d-charts-and-pies-vector4.jpg" width="10%" /></td>
                        </tr>
                    </table>
                    <br /><br /><br /><br /><br /><br />
                    <table>
                        <tr>
                            <td colspan="4" class="Input_Label_bold">Stato Istanze aperte verso il Comune <asp:Button runat="server" Cssclass="Bottone BottoneHelpMini" Text=""/></td>
                        </tr>
                        <tr class="CartListHead">
                            <td>Tributo</td>
                            <td>Tipo</td>
                            <td>Data di presentazione</td>
                            <td>Stato</td>
                        </tr>
                        <tr class="CartListItem">
                            <td>IMU</td>
                            <td>Modifica di una posizione accertata</td>
                            <td>26/03/2014</td>
                            <td>Registrata</td>
                        </tr>
                        <tr class="CartListItem">
                            <td>IMU</td>
                            <td>Dichiarazione di Variazione</td>
                            <td>09/07/2015</td>
                            <td>Presa in carico, in attesa di essere registrata</td>
                        </tr>
                        <tr class="CartListItem">
                            <td>TARI</td>
                            <td>Dichiarazione di Inagibilità</td>
                            <td>16/10/2015</td>
                            <td>In attesa di verifica</td>
                        </tr>
                    </table>
                </td>
            </tr>
		</table>
    </div>-->
    <!-- VIDEATA INIZIALE *** MERCATO SARACENO CAF-->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        &ensp;<asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br /><br />
        <asp:Label runat="server" CssClass="Input_Label_login">&ensp;HOMEPAGE</asp:Label>
    </div><br />
    <div style="width:100%">
        <table width="100%">
            <tr>
                <td colspan="2"></td>
                <td>Benvenuto/a <b>CAF SEDE PRINCIPALE</b></td>
            </tr>
            <tr valign="top">
                <td class="Input_Label_title" style="width:200px">
                    <table>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />Gestione Profilo</td>
                        </tr>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />Reports</td>
                        </tr>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />Corrispondenza con Ente</td>
                        </tr>
                        <tr>
                            <td class="Input_Label_title"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Width="30px" />F.A.Q.</td>
                        </tr>
                    </table>
                </td>
                <td>&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;</td>
                <td>Da questa pagina è possibile accedere alle schede informative sui tributi di pertinenza dell'Amministrazione comunale.<br />
                Si tratta, nel dettaglio:<br />
                    <table>
                        <tr>
                            <td>&ensp;</td>
                            <td>dell'Imposta Municipale Propria <u>(IMU)</u>;</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                        <tr>
                            <td>&ensp;</td>
                            <td>della Tassa sui rifiuti <u>(TARI)</u>;</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                    </table>
		            <br /><br /><br /><br /><br />
                    <p class="Input_Label_title">Elenco Contribuenti Associati</p>
                    <table>
                        <tr>
                            <td>Acme s.r.l.</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                        <tr>
                            <td>Daia Valerio</td>
                            <td><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="vai" Width="80px" /></td>
                        </tr>
                    </table>
                    <br /><br /><br /><br /><br /><br />
                    <table>
                        <tr>
                            <td colspan="4" class="Input_Label_bold">Stato Istanze aperte verso il Comune <asp:Button runat="server" Cssclass="Bottone BottoneHelpMini" Text=""/></td>
                        </tr>
                        <tr class="CartListHead">
                            <td>Tributo</td>
                            <td>Tipo</td>
                            <td>Contribuente</td>
                            <td>Data di presentazione</td>
                            <td>Stato</td>
                        </tr>
                        <tr class="CartListItem">
                            <td>IMU</td>
                            <td>Modifica di una posizione accertata</td>
                            <td>Daia Valerio</td>
                            <td>26/03/2014</td>
                            <td>Registrata</td>
                        </tr>
                        <tr class="CartListItem">
                            <td>TARI</td>
                            <td>Dichiarazione di Inagibilità</td>
                            <td>Daia Valerio</td>
                            <td>16/10/2015</td>
                            <td>In attesa di verifica</td>
                        </tr>
                    </table>
                </td>
            </tr>
		</table>
    </div>-->
    <!-- VIDEATA CRUSCOTTO ATTIVITA' - SINTETICA *** ATESSA-->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneGrafico" Text="  Grafico" Width="90px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneSort" Text="   Raffronto fra Enti" Width="170px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="     Stampa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Cruscotto Attività</asp:Label>
    </div><br />
    <div style="width:100%">
        <table>
            <tr>
                <td>Ente:</td>
                <td>Dal:</td>
                <td>Al:</td>
                <td rowspan="2"><input type="radio" /> Analitica<br />
                    <input type="radio" checked="checked" /> Sintetica<br />
                </td>
            </tr>
            <tr>
                <td>
                    <select class="Input_Text" style="width:200px">
                            <option value="1" selected="selected">Tutti</option>
                            <option value="2">&nbsp</option>
                    </select>
                </td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Istanze</p>
        <table>
            <tr class="CartListHead">
                <td style="width:300px">Stati</td>
                <td>N.</td>
            </tr>
            <tr class="CartListItem">
                <td>Registrazioni</td>
                <td>636</td>
                <td>&ensp;&ensp;&ensp;&ensp;&ensp;</td>
                <td rowspan="65">&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;<img alt="" src="../images/Bottoni/gara_chart_stati_istanze.png" width="50%" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Prese in carico</td>
                <td>5</td>
                <td><input type="text" style="background-color:aqua;width:20px;border:0" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Respinte all'utente</td>
                <td>99</td>
                <td><input type="text" style="background-color:red;width:20px;border:0" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Validate</td>
                <td>522</td>
                <td><input type="text" style="background-color:lime;width:20px;border:0" /></td>
            </tr>
            <tr class="CartListItem">
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <br /><br /><br /><br /><br />
        <table>
            <tr class="CartListHead">
                <td style="width:300px">Tempi Medi</td>
                <td>GG</td>
            </tr>
            <tr class="CartListItem">
				<td>Registrate - Prese in carico</td>
                <td>3</td>
                <td><input type="text" style="background-color:Orange;width:90px;border:0" /></td>
            </tr>
            <tr>
                <td>Registrate - Respinte all'utente</td>
                <td>5</td>
                <td><input type="text" style="background-color:Orange;width:150px;border:0" /></td>
            </tr>
            <tr>
                <td>Registrate sul verticale - Validate</td>
                <td>1</td>
                <td><input type="text" style="background-color:Orange;width:30px;border:0" /></td>
            </tr>
        </table>
    </div>-->
    <!-- VIDEATA CRUSCOTTO ATTIVITA' - SINTETICA *** MERCATO SARACENO -->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneGrafico" Text="     Grafico" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="     Stampa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Cruscotto Attività</asp:Label>
    </div><br />
    <div style="width:100%">
        <table>
            <tr>
                <td>Dal:</td>
                <td>Al:</td>
                <td rowspan="2"><input type="radio" /> Analitica<br />
                    <input type="radio" checked="checked" /> Sintetica<br />
                </td>
            </tr>
            <tr>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Istanze</p>
        <table>
            <tr class="CartListHead">
                <td style="width:300px">Stati</td>
                <td>N.</td>
            </tr>
            <tr class="CartListItem" style="height:30px">
                <td>Registrazioni</td>
                <td>636</td>
                <td>&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;</td>
                <td rowspan="8">&ensp;<div id="chart_div"></div></td>
            </tr>
            <tr class="CartListItem" style="height:30px">
                <td>Prese in carico</td>
                <td>15</td>
                <td><input type="text" style="background-color:aqua;width:20px;border:0" /></td>
                <td></td>
            </tr>
            <tr class="CartListItem" style="height:30px">
                <td>Respinte all'utente</td>
                <td>99</td>
                <td><input type="text" style="background-color:red;width:20px;border:0" /></td>
                <td></td>
            </tr>
            <tr class="CartListItem" style="height:30px">
                <td>Validate</td>
                <td>522</td>
                <td><input type="text" style="background-color:lime;width:20px;border:0" /></td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <br /><br /><br /><br /><br />
        <table>
            <tr class="CartListHead">
                <td style="width:300px">Tempi Medi</td>
                <td>GG</td>
            </tr>
            <tr class="CartListItem">
				<td>Registrate - Prese in carico</td>
                <td>3</td>
                <td><input type="text" style="background-color:Orange;width:90px;border:0" /></td>
            </tr>
            <tr>
                <td>Registrate - Respinte all'utente</td>
                <td>5</td>
                <td><input type="text" style="background-color:Orange;width:150px;border:0" /></td>
            </tr>
            <tr>
                <td>Registrate - Validate</td>
                <td>6</td>
                <td><input type="text" style="background-color:Orange;width:180px;border:0" /></td>
            </tr>
        </table>
    </div>-->
    <!-- VIDEATA CRUSCOTTO ATTIVITA' - RAFFRONTO ENTI *** ATESSA-->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneGrafico" Text="  Grafico" Width="90px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneSort" Text="   Raffronto fra Enti" Width="170px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="     Stampa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Cruscotto Attività</asp:Label>
    </div><br />
    <div style="width:100%">
        <table>
            <tr>
                <td>Ente:</td>
                <td>Dal:</td>
                <td>Al:</td>
                <td rowspan="2"><input type="radio" checked="checked" /> Analitica<br />
                    <input type="radio" /> Sintetica<br />
                </td>
            </tr>
            <tr>
                <td>
                    <select class="Input_Text" style="width:200px">
                            <option value="1" selected="selected">Tutti</option>
                            <option value="2">&nbsp</option>
                    </select>
                </td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Istanze</p>
        <table>
            <tr class="CartListHead">
                <td>Ente</td>
                <td>Registrazioni</td>
                <td>Accessi</td>
                <td colspan="2">Anagrafiche</td>
                <td colspan="2">TARI</td>
                <td colspan="2">IMU</td>
                <td colspan="2">OSAP</td>
                <td colspan="2">ICP</td>
            </tr>
            <tr class="CartListItem">
                <td>Atessa</td>
                <td>1356</td>
                <td>8745</td>
                <td>5</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>40</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>5</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>122</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>1</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Santa Maria Imbaro</td>
                <td>250</td>
                <td>3405</td>
                <td>12</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>31</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>62</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>4</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>6</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Lanciano</td>
                <td>4002</td>
                <td>7982</td>
                <td>4</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>49</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>38</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>42</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>8</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Castel Frentano</td>
                <td>325</td>
                <td>3795</td>
                <td>0</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>39</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>35</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>55</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>4</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
        </table>
        <br /><br /><br /><br /><br />
        <p class="Input_Label_title">Situazione Pagamenti</p>
        <table>
            <tr class="CartListHead">
                <td>Ente</td>
                <td colspan="2">OSAP</td>
                <td colspan="2">ICP</td>
            </tr>
            <tr class="CartListItem">
				<td>Atessa</td>
                <td style="width:100px">N. 35</td>
                <td style="width:100px">€ 479,73</td>
                <td style="width:100px">N. 1</td>
                <td style="width:100px">€ 197,77</td>
            </tr>
            <tr class="CartListItem">
				<td>Santa Maria Imbaro</td>
                <td>N. 30</td>
                <td>€ 411,2</td>
                <td>N. 15</td>
                <td>€ 2966,66</td>
            </tr>
            <tr class="CartListItem">
				<td>Lanciano</td>
                <td>N. 49</td>
                <td>€ 671,63</td>
                <td>N. 17</td>
                <td>€ 3362,22</td>
            </tr>
            <tr class="CartListItem">
				<td>Castel Frentano</td>
                <td>N. 36</td>
                <td>€ 493,44</td>
                <td>N. 12</td>
                <td>€ 2373,33</td>
            </tr>
        </table>
    </div>-->
    <!-- VIDEATA CRUSCOTTO ATTIVITA' - RAFFRONTO ENTI *** MERCATO SARACENO -->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneGrafico" Text="     Grafico" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="     Stampa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Cruscotto Attività</asp:Label>
    </div><br />
    <div style="width:100%">
        <table>
            <tr>
                <td>Dal:</td>
                <td>Al:</td>
                <td rowspan="2"><input type="radio" checked="checked" /> Analitica<br />
                    <input type="radio" /> Sintetica<br />
                </td>
            </tr>
            <tr>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
            </tr>
        </table>
        <p class="Input_Label_title">Situazione Istanze</p>
        <table style="width:550px">
            <tr class="CartListHead">
                <td>Ente</td>
                <td>Registrazioni</td>
                <td>Accessi</td>
                <td colspan="2">Anagrafiche</td>
                <td colspan="2">TARI</td>
                <td colspan="2">IMU</td>
            </tr>
            <tr class="CartListItem">
                <td>Mercato Saraceno</td>
                <td>1356</td>
                <td>8745</td>
                <td>5</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>40</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td>45</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
        </table>
        <br /><br /><br /><br /><br />
    </div>-->
    <!-- VIDEATA GESTIONE ISTANZE *** ATESSA-->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="     Stampa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Gestione Istanze</asp:Label>
    </div><br />
    <div style="width:100%">
        <table>
            <tr>
                <td>Ente:</td>
                <td>Data Presentazione: </td>
                <td>Tributo:</td>
                <td>Stato Istanza: </td>
            </tr>
            <tr>
                <td><select class="Input_Text" style="width:250px">
                            <option value="1" selected="selected">Atessa</option>
                            <option value="2">&nbsp</option>
                    </select></td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
                <td>
                    <select class="Input_Text" style="width:150px">
                            <option value="1" selected="selected">Tutti</option>
                            <option value="2">&nbsp</option>
                    </select>
                </td>
                <td><select class="Input_Text" style="width:250px">
                            <option value="1" selected="selected">Tutte</option>
                            <option value="2">&nbsp</option>
                    </select></td>
            </tr>
        </table><br />
        <table>
            <tr class="CartListHead">
                <td>Tributo</td>
                <td style="width:200px">Tipo Istanza</td>
                <td>Data Presentazione</td>
                <td style="width:200px">Utente</td>
                <td style="width:200px">Stato/Esito</td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td>TARI</td>
                <td>Richiesta di inagibilità</td>
                <td>16/10/2015</td>
                <td>Rossi Mario</td>
                <td>Registrata</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>IMU</td>
                <td>Richiesta uso gratuito</td>
                <td>16/10/2015</td>
                <td>Tiberi Sempronio</td>
                <td>Registrata</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>IMU</td>
                <td>Richiesta di Cessazione</td>
                <td>16/10/2015</td>
                <td>Tiberi Sempronio</td>
                <td>Registrata</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>TARI</td>                
                <td>Richiesta di cessazione</td>
                <td>15/10/2015</td>
                <td>Enobarbo Lucio</td>
                <td>Presa in carico</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>OSAP</td>                
                <td>Nuova dichiarazione</td>
                <td>14/10/2015</td>
                <td>Pertinace Giordano</td>
                <td>Respinta all'utente</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>ICP</td>                
                <td>Pagamento</td>
                <td>06/10/2015</td>
                <td>ACME s.r.l.</td>
                <td>Presa in carico</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td></td>                
                <td>Variazione Anagrafica</td>
                <td>06/10/2015</td>
                <td>Archiepi Malachia</td>
                <td>Validata</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>IMU</td>                
                <td>Modifica</td>
                <td>06/09/2015</td>
                <td>Daia Valerio</td>
                <td>Validata</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>IMU</td>                
                <td>Dichiarazione di Variazione</td>
                <td>09/07/2015</td>
                <td>Rossi Mario</td>
                <td>Presa in carico</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
        </table>
    </div>-->
    <!-- VIDEATA GESTIONE ISTANZE *** MERCATO SARACENO-->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="     Stampa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Gestione Istanze</asp:Label>
    </div><br />
    <div style="width:100%">
        <table>
            <tr>
                <td>Data Presentazione: </td>
                <td>Tributo:</td>
                <td>Stato Istanza: </td>
            </tr>
            <tr>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
                <td>
                    <select class="Input_Text" style="width:150px">
                            <option value="1" selected="selected">Tutti</option>
                            <option value="2">&nbsp</option>
                    </select>
                </td>
                <td><select class="Input_Text" style="width:250px">
                            <option value="1" selected="selected">Tutte</option>
                            <option value="2">&nbsp</option>
                    </select></td>
            </tr>
        </table><br />
        <table>
            <tr class="CartListHead">
                <td>Tributo</td>
                <td style="width:200px">Tipo Istanza</td>
                <td>Data Presentazione</td>
                <td style="width:200px">Utente</td>
                <td style="width:200px">Stato/Esito</td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td>TARI</td>
                <td>Richiesta di inagibilità</td>
                <td>16/10/2015</td>
                <td>Rossi Mario</td>
                <td>Registrata</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>IMU</td>
                <td>Richiesta uso gratuito</td>
                <td>16/10/2015</td>
                <td>Tiberi Sempronio</td>
                <td>Registrata</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>IMU</td>
                <td>Richiesta di cessazione</td>
                <td>16/10/2015</td>
                <td>Tiberi Sempronio</td>
                <td>Registrata</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>TARI</td>                
                <td>Richiesta di cessazione</td>
                <td>15/10/2015</td>
                <td>Enobarbo Lucio</td>
                <td>Presa in carico</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>TARI</td>                
                <td>Richiesta di modifica</td>
                <td>14/10/2015</td>
                <td>Pertinace Giordano</td>
                <td>Respinta all'utente</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>TARI</td>                
                <td>Dichiarazione di variazione</td>
                <td>06/10/2015</td>
                <td>ACME s.r.l.</td>
                <td>Presa in carico</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td></td>                
                <td>Variazione Anagrafica</td>
                <td>06/10/2015</td>
                <td>Archiepi Malachia</td>
                <td>Validata</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>IMU</td>                
                <td>Richiesta di modifica</td>
                <td>06/09/2015</td>
                <td>Daia Valerio</td>
                <td>Validata</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>IMU</td>                
                <td>Dichiarazione di variazione</td>
                <td>09/07/2015</td>
                <td>Rossi Mario</td>
                <td>Presa in carico</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
        </table>
    </div>-->
    <!-- VIDEATA CRUSCOTTO-->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneGrafico" Text="     Grafico" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="     Stampa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Cruscotto</asp:Label>
    </div><br />
    <div style="width:100%">
        <table>
            <tr>
                <td>Ente:</td>
                <td>Dal:</td>
                <td>Al:</td>
                <td rowspan="3"><input type="radio" /> Raffronto<br />
                    <input type="radio" /> Analitica<br />
                    <input type="radio" checked="checked" /> Sintetica<br />
                </td>
                <td>Tipo Periodo:</td>
                <td>Tipo Accesso: </td>
            </tr>
            <tr>
                <td><select class="Input_Text" style="width:250px">
                            <option value="1" selected="selected">Atessa</option>
                            <option value="2">&nbsp</option>
                    </select></td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>&nbsp
                    <select class="Input_Text" style="width:50px">
                            <option value="1" selected="selected">...</option>
                            <option value="2">&nbsp</option>
                    </select></td>
                <td><select class="Input_Text" style="width:150px">
                            <option value="1" selected="selected">Tutte</option>
                            <option value="2">&nbsp</option>
                    </select></td>
            </tr>
            <tr><td><br /></td></tr>
        </table><br />
        <table>
            <tr class="CartListHead">
                <td style="width:200px">Tipo Accesso</td>
                <td>Numero</td>
            </tr>
            <tr class="CartListItem">
                <td>Consultazione</td>
                <td>3256</td>
            </tr>
            <tr class="CartListItem">
                <td>Istanza Anagrafica</td>                
                <td>161</td>
            </tr>
            <tr class="CartListItem">
                <td>Istanza ICP</td>
                <td>125</td>
            </tr>
            <tr class="CartListItem">
                <td>Istanza IMU</td>
                <td>985</td>
            </tr>
            <tr class="CartListItem">
                <td>Istanza OSAP</td>                
                <td>210</td>
            </tr>
            <tr class="CartListItem">
                <td>Istanza TARI</td>                
                <td>985</td>
            </tr>
            <tr class="CartListItem">
                <td>Istanza di Pagamento</td>                
                <td>561</td>
            </tr>
        </table>
    </div>-->
	<!-- VIDEATA DETTAGLIO ISTANZA -->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneStop" Text="    Respingi" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneAccept" Text="   Valida" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneAnnulla" Text="     Annulla" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Dettaglio Istanza</asp:Label>
    </div><br />
    <div style="width:100%">
        <p class="Input_Label_title">Dati Istanza</p>
        <table style="border:2px solid #0066CC;border-radius: 20px;padding:25px;width:750px">
            <tr>
                <td>Ente:</td>
                <td>Tributo:</td>
                <td>Tipo Istanza:</td>
                <td>Data Presentazione: </td>
                <td>Stato Istanza: </td>
                <td>Motivazione:</td>
            </tr>
            <tr>
                <td>Atessa</td>
                <td>IMU</td>
                <td>Richiesta di cessazione</td>
                <td>16/10/2015</td>
                <td>Registrata</td>
                <td></td>
            </tr>
        </table>
        <p class="Input_Label_title">Dati Contribuente</p>
        <table style="border:2px solid #0066CC;border-radius: 20px;padding:25px;width:750px">
            <tr>
                <td>Cognome:</td>
                <td>Nome:</td>
                <td>Codice Fiscale/P.IVA:</td>
                <td>Altri dati Anagrafici...</td>
            </tr>
            <tr>
                <td>Tiberi</td>
                <td>Sempronio</td>
                <td>TBRSPR36E04B519Y</td>
                <td>...</td>
            </tr>
        </table>
        <p class="Input_Label_title">Dettaglio Istanza</p>
        <table style="border:2px solid #0066CC;border-radius: 20px;padding:25px;width:750px">
            <tr>
                <td colspan="2">Dati specifici per Tributo:</td>
            </tr>
            <tr>
                <td colspan="2">...</td>
            </tr>
            <tr>
                <td>Data di Cessazione:</td>
                <td>Note:</td>
            </tr>
            <tr>
                <td>30/09/2015</td>
                <td></td>
            </tr>
        </table>
        <p class="Input_Label_title">Allegati</p>
        <table style="border:2px solid #0066CC;border-radius: 20px;padding:25px;width:750px">
            <tr class="CartListHead">
                <td colspan="5">Documento</td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td colspan="5">Ricevuta registrazione dichiarazione</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td colspan="5">Atto di vendita immobile</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
        </table>
    </div>-->    
    <!-- VIDEATA CONFIGURAZIONE -->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        <asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Width="30px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Width="30px"/>
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="     Stampa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneNewInsert" Text="   Nuovo" Width="90px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Configurazione</asp:Label>
    </div><br />
    <div style="width:100%">
        <table>
            <tr>
                <td>Ente:</td>
                <td>Anno: </td>
            </tr>
            <tr>
                <td><select class="Input_Text" style="width:250px">
                            <option value="1" selected="selected">Atessa</option>
                            <option value="2">&nbsp</option>
                    </select></td>
                <td><asp:TextBox runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox></td>
            </tr>
        </table><br />
        <table>
            <tr class="CartListHead">
                <td>Codice</td>
                <td style="width:300px">Descrizione</td>
                <td></td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td>A001</td>
                <td>Valore configurato 1</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneCancella" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>A002</td>
                <td>Valore configurato 2</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneCancella" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>A003</td>
                <td>Valore configurato 3</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneCancella" style="width:30px;height:30px" /></td>
            </tr>
        </table>
    </div> -->
    <!-- COMANDI -->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
		<table cellspacing="20">
            <tr>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneHome" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneHelp" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneSalva" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneAnnulla" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneAttention" Text="" Height="40px" Width="40px"/></td>
            </tr>
            <tr>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneSoggetti" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneModifica" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneNewInsert" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneRettificaAvviso" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneApri" Text="" Height="40px" Width="40px"/></td>
            </tr>
            <tr>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneApriElaborazioni" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneClose" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="" Height="40px" Width="40px"/></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneGoTo" Text="" Height="40px" Width="40px"/></td>
            </tr>
            <tr>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneSort" Text="" Height="40px" Width="40px" /></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneGrafico" Text="" Height="40px" Width="40px" /></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneAccept" Text="" Height="40px" Width="40px" /></td>
                <td align="center"><asp:Button runat="server" Cssclass="Bottone BottoneStop" Text="" Height="40px" Width="40px" /></td>
                <td align="center"></td>
            </tr>
        </table>
    </div> -->
    <!-- COMPENSAZIONI CREA - VIDEATA RICERCA -->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneOk" Text="     Fine Fase" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneExcel" Text="     Stampa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Compensazioni - Crea</asp:Label>
    </div><br />
    <div style="width:100%">
        <table>
            <tr>
                <td>Tributo</td>
                <td>
                    <select class="Input_Text" style="width:150px">
                            <option value="1" selected="selected"></option>
                            <option value="2">&nbsp</option>
                    </select>
                </td>
                <td>Anno</td>
                <td>
                    <asp:TextBox runat="server" CssClass="Input_Text" style="width:80px"></asp:TextBox>
                </td>
                <td>Nominativo</td>
                <td>
                    <asp:TextBox runat="server" CssClass="Input_Text" style="width:300px"></asp:TextBox>
                </td>
                <td>Cod.Fiscale/P.IVA</td>
                <td>
                    <asp:TextBox runat="server" CssClass="Input_Text" style="width:160px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <p class="Input_Label_title">Risultati della ricerca</p>
        <table>
            <tr class="CartListHead">
                <td style="width:300px">Nominativo</td>
                <td style="width:160px">Cod.Fiscale/P.Iva</td>
                <td>Anno</td>
                <td style="width:90px">Tributo</td>
                <td style="width:100px">Avviso</td>
                <td>Importo</td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td>Daia Valerio</td>
                <td>DAIVLR56H15B026H</td>
                <td>2014</td>
                <td>TARES</td>
                <td>44850</td>
                <td>75,00</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Enobarbo Lucio</td>
                <td>NBRLCU86E25B973U</td>
                <td>2014</td>
                <td>TASI</td>
                <td></td>
                <td>38,00</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Pertinace Giordano</td>
                <td>PRTGDN52A30E319J</td>
                <td>2014</td>
                <td>TARES</td>
                <td>38281</td>
                <td>62,50</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Tiberi Sempronio</td>
                <td>TBRSPR32C02Z116R</td>
                <td>2013</td>
                <td>IMU</td>
                <td></td>
                <td>15,50</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
        </table>
    </div>-->
    <!-- COMPENSAZIONI CREA - VIDEATA GESTIONE -->
    <!--<div style="float:right;width:100%" class="SfondoGenerale">
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <asp:Button runat="server" Cssclass="Bottone BottoneSalva" Text="     Compensa" Width="100px"/>
        <asp:Button runat="server" Cssclass="Bottone BottoneRicerca" Text="Ricerca" Width="100px"/>
    </div><br />
    <div class="SfondoGenerale" style="height:50px"><br />
        <asp:Label runat="server" CssClass="Input_Label_login">Compensazioni - Gestione</asp:Label>
    </div><br />
    <div style="width:100%">
        <fieldset class="FiledSetRicerca">
            <p class="Legend">Enobarbo Lucio&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
            Cod.Fiscale/P.Iva NBRLCU86E25B973U<br />
            Anno 2014&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
            Tributo TASI&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
            Importo da compensare 38,00€</p>
        </fieldset><br />
        <fieldset class="FiledSetRicerca"><legend class="Legend">Parametri ricerca</legend>
            <table>
                <tr>
                    <td>Nominativo</td>
                    <td>
                        <asp:TextBox runat="server" CssClass="Input_Text" style="width:300px">Enobarbo Lucio</asp:TextBox>
                    </td>
                    <td>Cod.Fiscale/P.IVA</td>
                    <td>
                        <asp:TextBox runat="server" CssClass="Input_Text" style="width:160px">NBRLCU86E25B973U</asp:TextBox>
                    </td>
                    <td>Tributo</td>
                    <td>
                        <select class="Input_Text" style="width:150px">
                                <option value="1" selected="selected"></option>
                                <option value="2">&nbsp</option>
                        </select>
                    </td>
                    <td>Anno</td>
                    <td>
                        <asp:TextBox runat="server" CssClass="Input_Text" style="width:80px"></asp:TextBox>
                    </td>
                    
                </tr>
            </table>
        </fieldset>
        <p class="Input_Label_title">Risultati della ricerca</p>
        <table>
            <tr class="CartListHead">
                <td style="width:300px">Nominativo</td>
                <td style="width:160px">Cod.Fiscale/P.Iva</td>
                <td>Anno</td>
                <td style="width:90px">Tributo</td>
                <td style="width:100px">Avviso</td>
                <td>Dovuto</td>
                <td>Pagato</td>
                <td>Interessi</td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td>Enobarbo Lucio</td>
                <td>NBRLCU86E25B973U</td>
                <td>2015</td>
                <td>TARES</td>
                <td>1235</td>
                <td>320,00</td>
                <td>0,00</td>
                <td><asp:CheckBox runat="server" CssClass="Input_CheckBox_NoBorder" Checked="true" /></td>
                <td><asp:CheckBox runat="server" CssClass="Input_CheckBox_NoBorder" Checked="true" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Enobarbo Lucio</td>
                <td>NBRLCU86E25B973U</td>
                <td>2014</td>
                <td>TARES</td>
                <td>5121</td>
                <td>380,00</td>
                <td>190,00</td>
                <td><asp:CheckBox runat="server" CssClass="Input_CheckBox_NoBorder" /></td>
                <td><asp:CheckBox runat="server" CssClass="Input_CheckBox_NoBorder" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Enobarbo Lucio</td>
                <td>NBRLCU86E25B973U</td>
                <td>2014</td>
                <td>IMU</td>
                <td></td>
                <td>162,50</td>
                <td>81,00</td>
                <td><asp:CheckBox runat="server" CssClass="Input_CheckBox_NoBorder" /></td>
                <td><asp:CheckBox runat="server" CssClass="Input_CheckBox_NoBorder" /></td>
            </tr>
        </table>
    </div>-->    
    <!-- VIDEATA controllo attività *** POMARANCE-->
    <div style="width:100%">
        <br /><br /><br />
        <table>
            <tr class="CartListHead">
                <td style="width:200px">Attività da U.Commercio</td>
                <td>Gestite</td>
                <td>Non Gestite</td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td>Nuove attività</td>
                <td>35</td>
                <td>3</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Attività Variate</td>
                <td>12</td>
                <td>16</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
            <tr class="CartListItem">
                <td>Attività Cessate</td>
                <td>10</td>
                <td>0</td>
                <td><asp:Button runat="server" Cssclass="Bottone BottoneApri" style="width:30px;height:30px" /></td>
            </tr>
        </table>
    </div>
    <div style="position:absolute;top:80px; left:350px; border:2px solid #003399;background-color:#fff">
        <table>
            <tr class="CartListItem">
                <td colspan="3">
                    Dettaglio Nuove Attività Non Gestite
                </td>
            </tr>
            <tr class="CartListHead">
                <td style="width:150px">Partita Iva</td>
                <td>Rif.Cat.</td>
                <td>Nota</td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td>06500158516</td>
                <td>12/150/3</td>
                <td></td>
            </tr>
            <tr class="CartListItem">
                <td>80031042512</td>
                <td>31/5/1</td>
                <td>Ai fini TARI attività coperta da P.IVA: 00215045721</td>
            </tr>
            <tr class="CartListItem">
                <td>00000647897</td>
                <td>9/56/</td>
                <td></td>
            </tr>
        </table>
    </div>
    <br /><br /><br /><br /><br />
    </form>
</body>
</html>
