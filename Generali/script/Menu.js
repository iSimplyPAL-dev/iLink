$(document).ready(function () {
    LoadMenu("../script/xmlMenu.xml");
});
function LoadMenu(urlXML) {
    $.ajax({
        type: "GET",
        url: urlXML,
        dataType: "xml",
        success: function (xml) {
            var ulFirst = $("<ul />");
            $(xml).find("Menu").each(function () {
                if ($(this).children().length) {
                    var ulSecond = $("<ul />");
                    $(this).children().each(function () {
                        if ($(this).children().length) {
                            var ulThird = $("<ul />");
                            $(this).children().each(function () {
                                if ($(this).children().length) {
                                    var ulFourth = $("<ul />");
                                    $(this).children().each(function () {
                                        if ($(this).children().length) {
                                            var ulFifth = $("<ul />");
                                            $(this).children().each(function () {
                                                ulFifth.append("<li class=\"Menu MenuFifth\" id=" + $(this).attr("id") + "><a id=\"lnk" + $(this).attr("id") + "\" class=\"Menu\" onclick=\"LinkMenu('lnk" + $(this).attr("id") + "','" + $(this).attr("NomeEnte") + "','" + $(this).attr("urlVisualizza") + "','" + $(this).attr("urlComandi") + "','" + $(this).attr("urlBasso") + "','" + $(this).attr("urlNascosto") + "')\">" + $(this).attr("text") + "</a></li>");
                                            });
                                            var liFourth = $("<li class=\"Menu MenuFourth\" id=" + $(this).attr("id") + "><a id=\"lnk" + $(this).attr("id") + "\" class=\"Menu\" onclick=\"LinkMenu('lnk" + $(this).attr("id") + "','" + $(this).attr("NomeEnte") + "','" + $(this).attr("urlVisualizza") + "','" + $(this).attr("urlComandi") + "','" + $(this).attr("urlBasso") + "','" + $(this).attr("urlNascosto") + "')\">" + $(this).attr("text") + "</a></li>");
                                            ulFirst.append(ulSecond.append(ulThird.append(ulFourth.append(liFourth.append(ulFifth)))));
                                        }
                                        else
                                            ulFourth.append("<li class=\"Menu MenuFourth\" id=" + $(this).attr("id") + "><a id=\"lnk" + $(this).attr("id") + "\" class=\"Menu\" onclick=\"LinkMenu('lnk" + $(this).attr("id") + "','" + $(this).attr("NomeEnte") + "','" + $(this).attr("urlVisualizza") + "','" + $(this).attr("urlComandi") + "','" + $(this).attr("urlBasso") + "','" + $(this).attr("urlNascosto") + "')\">" + $(this).attr("text") + "</a></li>");
                                    });
                                    var liThird = $("<li class=\"Menu MenuThird\" id=" + $(this).attr("id") + "><a id=\"lnk" + $(this).attr("id") + "\" class=\"Menu\" onclick=\"LinkMenu('lnk" + $(this).attr("id") + "','" + $(this).attr("NomeEnte") + "','" + $(this).attr("urlVisualizza") + "','" + $(this).attr("urlComandi") + "','" + $(this).attr("urlBasso") + "','" + $(this).attr("urlNascosto") + "')\">" + $(this).attr("text") + "</a></li>");
                                    ulFirst.append(ulSecond.append(ulThird.append(liThird.append(ulFourth))));
                                }
                                else
                                    ulThird.append("<li class=\"Menu MenuThird\" id=" + $(this).attr("id") + "><a id=\"lnk" + $(this).attr("id") + "\" class=\"Menu\" onclick=\"LinkMenu('lnk" + $(this).attr("id") + "','" + $(this).attr("NomeEnte") + "','" + $(this).attr("urlVisualizza") + "','" + $(this).attr("urlComandi") + "','" + $(this).attr("urlBasso") + "','" + $(this).attr("urlNascosto") + "')\">" + $(this).attr("text") + "</a></li>");
                            });
                            var liSecond = $("<li class=\"Menu MenuSecond\" id=" + $(this).attr("id") + "><a id=\"lnk" + $(this).attr("id") + "\" class=\"Menu\" onclick=\"LinkMenu('lnk" + $(this).attr("id") + "','" + $(this).attr("NomeEnte") + "','" + $(this).attr("urlVisualizza") + "','" + $(this).attr("urlComandi") + "','" + $(this).attr("urlBasso") + "','" + $(this).attr("urlNascosto") + "')\">" + $(this).attr("text") + "</a></li>");
                            ulFirst.append(ulSecond.append(liSecond.append(ulThird)));
                        }
                        else
                            ulSecond.append("<li class=\"Menu MenuSecond\" id=" + $(this).attr("id") + "><a id=\"lnk" + $(this).attr("id") + "\" class=\"Menu\" onclick=\"LinkMenu('lnk" + $(this).attr("id") + "','" + $(this).attr("NomeEnte") + "','" + $(this).attr("urlVisualizza") + "','" + $(this).attr("urlComandi") + "','" + $(this).attr("urlBasso") + "','" + $(this).attr("urlNascosto") + "')\">" + $(this).attr("text") + "</a></li>");
                    });
                    var liFirst = $("<li class=\"Menu MenuFirst\" id=" + $(this).attr("id") + "><a id=\"lnk" + $(this).attr("id") + "\" class=\"Menu\" onclick=\"LinkMenu('lnk" + $(this).attr("id") + "','" + $(this).attr("NomeEnte") + "','" + $(this).attr("urlVisualizza") + "','" + $(this).attr("urlComandi") + "','" + $(this).attr("urlBasso") + "','" + $(this).attr("urlNascosto") + "')\">" + $(this).attr("text") + "</a></li>");
                    ulFirst.append(liFirst.append(ulSecond));
                }
                else
                    ulFirst.append("<li class=\"Menu MenuFirst\" id=" + $(this).attr("id") + "><a id=\"lnk" + $(this).attr("id") + "\" class=\"Menu\" onclick=\"LinkMenu('lnk" + $(this).attr("id") + "','" + $(this).attr("NomeEnte") + "','" + $(this).attr("urlVisualizza") + "','" + $(this).attr("urlComandi") + "','" + $(this).attr("urlBasso") + "','" + $(this).attr("urlNascosto") + "')\">" + $(this).attr("text") + "</a></li>");
            });
            $('#menu_wrapper').append(ulFirst);
            $('.MenuSecond').hide();
            $('.MenuThird').hide();
            $('.MenuFourth').hide();
            $('.MenuFifth').hide();
        }
    });
}

function LinkMenu(IDElement, NomeEnte, Visualizza, Comandi, Basso, Nascosto) {
    console.log('ho questo visualizza->' + Visualizza);
    console.log('ho questo Comandi->' + Comandi);
    console.log('sono->' + IDElement);
    if (Visualizza != '' && Visualizza != 'undefined') {
        parent.Nascosto.location.href = Nascosto;
        parent.Visualizza.location.href = Visualizza.replace('myidente',$('#hfIdEnte').val());
        parent.Comandi.location.href = Comandi.replace('myidente', $('#hfIdEnte').val());
        parent.Basso.location.href = Basso;
    }
    else {
        if ($('#' + IDElement).closest('li').find('.MenuSecond').is(':visible')){
            $('#' + IDElement).closest('li').find('.MenuSecond').hide();
        }
        else {
            $('#' + IDElement).closest('li').find('.MenuSecond').show();
        }
        if ($('#' + IDElement).closest('li').hasClass('MenuSecond'))
        {
            if ($('#' + IDElement).closest('li').find('.MenuThird').is(':visible')) {
                $('#' + IDElement).closest('li').find('.MenuThird').hide();
            }
            else {
                $('#' + IDElement).closest('li').find('.MenuThird').show();
            }
        }
        if ($('#' + IDElement).closest('li').hasClass('MenuThird'))
        {
            if ($('#' + IDElement).closest('li').find('.MenuFourth').is(':visible')) {
                $('#' + IDElement).closest('li').find('.MenuFourth').hide();
            }
            else {
                $('#' + IDElement).closest('li').find('.MenuFourth').show();
            }
        }
        if ($('#' + IDElement).closest('li').hasClass('MenuFourth'))
        {
            if ($('#' + IDElement).closest('li').find('.MenuFifth').is(':visible')) {
                $('#' + IDElement).closest('li').find('.MenuFifth').hide();
            }
            else {
                $('#' + IDElement).closest('li').find('.MenuFifth').show();
            }
        }
        if (IDElement.match('lnkENTE')) {
            console.log('scelto ente');
            $('#hfIdEnte').val(IDElement.replace('lnkENTE', ''));
            $('#hfDescrizioneEnte').val(NomeEnte);
            console.log('ora sono->' + $('#hfIdEnte').val());
            $('#CmdLoadMenuEnte').click();
            parent.Nascosto.location.href = '../../aspSvuota.aspx';
            parent.Basso.location.href = '../../aspVuota.aspx';
            parent.Visualizza.location.href = '../../aspVuota.aspx';
            parent.Comandi.location.href = '../asp/aspComandiVuota.aspx';
        }
        if (IDElement.match('lnk0434') || IDElement.match('lnk8852') || IDElement.match('lnk0453') || IDElement.match('lnk9763')) {
            $('#hfIdTributo').val(IDElement.replace('lnk', ''));
        }
    }
}

function Link(Tipo_ProcServ, IDProcServ, CODProcServ, Desc_ProcServ) {
    Parametri = "ProcServ=" + IDProcServ + "&Tipo_ProcServ=" + Tipo_ProcServ + "&Desc_ProcServ=" + Desc_ProcServ
    parent.frames.item("visualizza").location.href = "../asp/aspFrameProcServ.aspx?" + Parametri
}

function OverSottoMenu(id){
	id.className="SottoMenuOver"
	id.style.textDecoration="underline"
	window.status="- " + id.innerText + " -" 
}

function OutSottoMenu(id){
	id.className="SottoMenu"
	id.style.textDecoration="none"
	window.status="Done"
}

function Esci(){
    if (confirm('Uscire dall\'applicativo?')) {
        $('#CmdLogOut').click();
	}
}