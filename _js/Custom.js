$(document).ready(function () {
    LoadTooltip();
    $('*[class^="Input_Text"]').blur(function () {
        console.log('controllo');
        if ($('#' + $(this).attr('id')).val().indexOf('../') >= 0 || $('#' + $(this).attr('id')).val().indexOf('<') >= 0 || $('#' + $(this).attr('id')).val().indexOf('>') >= 0) {
            GestAlert('a', 'warning', '', '', 'Presenza di caratteri non validi! Non sono accettati ../ < >');
            $('#' + $(this).attr('id')).focus();
            return false;
        }
    });
    $('#Username').blur(function () {
        $('#lblMessage').text('');
        $('#lblMessage').hide();
        console.log('sono->#' + $(this).attr('id'));
        if ($('#' + $(this).attr('id')).val().indexOf('../') >= 0 || $('#' + $(this).attr('id')).val().indexOf('<') >= 0 || $('#' + $(this).attr('id')).val().indexOf('>') >= 0) {
            $('#lblMessage').text('Presenza di caratteri non validi! Non sono accettati ../ < >');
            $('#lblMessage').show(); //Show error
            $('#' + $(this).attr('id')).text('');
            $('#' + $(this).attr('id')).focus();
            return false;
        }
    });
    $('#Password').blur(function () {
        $('#lblMessage').text('');
        $('#lblMessage').hide();
        console.log('sono->#' + $(this).attr('id'));
        if ($('#' + $(this).attr('id')).val().indexOf('../') >= 0 || $('#' + $(this).attr('id')).val().indexOf('<') >= 0 || $('#' + $(this).attr('id')).val().indexOf('>') >= 0) {
            $('#lblMessage').text('Presenza di caratteri non validi! Non sono accettati ../ < >');
            $('#lblMessage').show(); //Show error
            $('#' + $(this).attr('id')).text('');
            $('#' + $(this).attr('id')).focus();
            return false;
        }
    });
    $(".OnlyNumber").keypress(function (e) {
        var chr = String.fromCharCode(e.which);
        if ("0123456789., 	".indexOf(chr) < 0) {
            console.log('sono su:' + chr + 'FINE');
            console.log('che è su:' + e.which + 'FINE');
            GestAlert('a', 'warning', '', '', 'Inserire solo Numeri!');
            $('#' + $(this).attr('id')).text('');
            $('#' + $(this).attr('id')).focus();
            return false;
        }
    });
    $(".OnlyNumber").blur(function () {
        var chr = $(".OnlyNumber").val();
        chr = chr.replace(',', '').replace('.', '').replace(' ', '');
        if (chr !== "" && !$.isNumeric(chr)) {
            GestAlert('a', 'warning', '', '', 'Inserire solo Numeri!');
            console.log(chr);
            $('#' + $(this).attr('id')).text('');
            $('#' + $(this).attr('id')).focus();
            return false;
        }
    });
    $(".TextDate").keypress(function (e) {
        var chr = String.fromCharCode(e.which);
        if ("0123456789/".indexOf(chr) < 0) {
            GestAlert('a', 'warning', '', '', 'Inserire solo Numeri o /!');
            $('#' + $(this).attr('id')).text('');
            $('#' + $(this).attr('id')).focus();
            return false;
        }
    });
})
function LoadTooltip() {
    //*** tooltip generici ***
    $(".BottoneApri").hover(
        function () {
            var descr = "Apri"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $("p#apri").html("Apri");
}
function ShowHideGrdBtn(idControllo) {
    if ($('#' + idControllo).is(':checked')) {
        $('#' + idControllo).closest('td').find('.divGrdBtn').show();
    }
    else {
        $('#' + idControllo).closest('td').find('.divGrdBtn').hide();
    }
    if ($('#' + idControllo).closest('td').find('.divGrdBtn').find('input:visible').length <= 3)
        $('.panelGrd').height(50);
    else if ($('#' + idControllo).closest('td').find('.divGrdBtn').find('input:visible').length <= 6)
        $('.panelGrd').height(125);
}
// #region DialogBoxe
function GestAlert(typeAlert, gravityAlert, buttonOK, buttonKO, messageAlert) {
    /// <summary>Funzione per la gestione delle finestre di dialogo/avviso.</summary>  
    /// <param name="typeAlert" type="string">può assumere i seguenti valori <list type="bullet"><item><c>a</c> alert</item><item><c>c</c> confirm</item><item><c>p</c> prompt</item></list></param>  
    /// <param name="gravityAlert" type="string">può assumere i seguenti valori <list type="bullet"><item><c>danger</c> in caso di errore</item><item><c>warning</c> in caso di avviso bloccante</item><item><c>success</c> in caso di nuon fine</item><item><c>info</c> in caso di semplice notifica</item><item><c>question</c> in caso di domanda</item></list></param>  
    /// <param name="buttonOK" type="string">è il nome del pulsante da lanciare quando si clicca il pulsante ok oppure il nome del pulsante da lanciare con le istruzioni da eseguire dopo l'alert</param>  
    /// <param name="buttonKO" type="string">è il nome del pulsante da lanciare quando si clicca il pulsante ko</param>  
    /// <param name="messageAlert" type="string">è il testo da visualizzare</param>  
    if (typeAlert == 'c') {
        var HeightComandi = 0;
        if ($(parent.parent.frames["Comandi"]) != undefined)
            HeightComandi = $(parent.parent.frames["Comandi"]).height();
        else if ($(parent.frames["Comandi"]) != undefined)
            HeightComandi = $(parent.frames["Comandi"]).height();
        console.log('alto=' + HeightComandi);
        $('input#cmdHeight', parent.frames["DialogBoxe"].document).val(HeightComandi);
        $('#divAlert p', parent.frames["DialogBoxe"].document).html(messageAlert);
        $('#divAlert', parent.frames["DialogBoxe"].document).removeClass();
        $('#divAlert', parent.frames["DialogBoxe"].document).addClass('modal-alert');
        $('#divAlert', parent.frames["DialogBoxe"].document).addClass(gravityAlert);
        $('#divAlert', parent.frames["DialogBoxe"].document).css({ opacity: 1 });
        $('#hfDialogOK', parent.frames["DialogBoxe"].document).val(buttonOK);
        $('#hfDialogKO', parent.frames["DialogBoxe"].document).val(buttonKO);
        $('.confirmbtn', parent.frames["DialogBoxe"].document).show();
        $('.prompttxt', parent.frames["DialogBoxe"].document).hide();
        $('.modal-box', parent.frames["DialogBoxe"].document).show();
        parent.parent.document.getElementById('frameVisualizza').rows = '0,0,*,0,0';
    }
    else {
        var HeightComandi = 0;
        if ($(parent.parent.frames["Comandi"]) != undefined)
            HeightComandi = $(parent.parent.frames["Comandi"]).height();
        else if ($(parent.frames["Comandi"]) != undefined)
            HeightComandi = $(parent.frames["Comandi"]).height();
        console.log('alto=' + HeightComandi);
        $('input#cmdHeight').val(HeightComandi);
        $('#divAlert p').html(messageAlert);
        $('#divAlert').removeClass();
        $('#divAlert').addClass('modal-alert');
        $('#divAlert').addClass(gravityAlert);
        $('#divAlert').css({ opacity: 1 });
        $('#hfCloseAlert').val(buttonOK);
        $('.confirmbtn').hide();
        $('.modal-box').show();
    }
    if (typeAlert == 'p')
        $('.prompttxt').show();
    else
        $('.prompttxt').hide();
}
function CloseAlert() {
    $('.modal-box').hide();
    if ($('#hfCloseAlert').val() != '') {
        console.log('eseguo dopo chiusura');
        if ($('#hfCloseAlert').val() == 'CmdLogOut')
            $('#' + $('#hfCloseAlert').val(), parent.parent.frames["viste"].document).click();
        else
            $('#' + $('#hfCloseAlert').val()).click();
    }
}
function DialogConfirmOK() {
    CloseAlert();
    $('.modal-box').hide();
    if (parseInt($('input#cmdHeight').val()) > 10) {
        parent.parent.document.getElementById('frameVisualizza').rows = '45,*,0,0,0';
    }
    else {
        parent.parent.document.getElementById('frameVisualizza').rows = '0,*,0,0,0';
    }
    $('#CmdDialogConfirmOK').click();
}
function DialogConfirmKO() {
    CloseAlert();
    $('.modal-box').hide();
    if (parseInt($('input#cmdHeight').val()) > 10) {
        parent.parent.document.getElementById('frameVisualizza').rows = '45,*,0,0,0';
    }
    else {
        parent.parent.document.getElementById('frameVisualizza').rows = '0,*,0,0,0';
    }
    $('#CmdDialogConfirmKO').click();
}
function RaiseDialogConfirmOK() {
    console.log('click ok per->' + $('#hfDialogOK').val());
    if ($('#hfDialogOK').val() != '')
        if ($('#hfDialogOK').val() == 'CmdLogOut')
            $('#' + $('#hfDialogOK').val(), parent.parent.frames["viste"].document).click();
        else
            if ($('#' + $('#hfDialogOK').val(), parent.frames["Visualizza"].document) == "undefined")
                $('#' + $('#hfDialogOK').val()).click();
            else
                $('#' + $('#hfDialogOK').val(), parent.frames["Visualizza"].document).click();
}
function RaiseDialogConfirmKO() {
    CloseAlert();
    if ($('#hfDialogKO').val() != '')
        $('#' + $('#hfDialogKO').val()).click();
}
// #endregion
function LoadOpenWindow(mysrc) {
    /// <summary>Funzione per l'apertura di un popup in frame anzichè nuova pagina.</summary>  
    /// <param name="mysrc" type="string">contiene l'url della pagina da visualizzare completa di parametri</param>  
    var HeightComandi = 0;
    if ($(parent.parent.frames["Comandi"]) != undefined)
        HeightComandi = $(parent.parent.frames["Comandi"]).height();
    else if ($(parent.frames["Comandi"]) != undefined)
        HeightComandi = $(parent.frames["Comandi"]).height();
    console.log('alto=' + HeightComandi);
    console.log('devo aprire->' + mysrc);
    $('input#cmdHeight', parent.parent.frames["DialogBoxe"].document).val(HeightComandi);
    $('#ifrLoadWindow', parent.parent.frames["DialogBoxe"].document).attr('src', mysrc)
    $('#divAlert', parent.parent.frames["DialogBoxe"].document).removeClass();
    $('#divAlert', parent.parent.frames["DialogBoxe"].document).addClass('hidden');
    $('#divLoadWindow', parent.parent.frames["DialogBoxe"].document).removeClass();
    $('#divAlert', parent.parent.frames["DialogBoxe"].document).addClass('modal-alert');
    $('#divAlert', parent.parent.frames["DialogBoxe"].document).css({ opacity: 1 });
    $('.confirmbtn').hide();
    $('.modal-box').show();
    parent.parent.parent.document.getElementById('frameVisualizza').rows = '0,0,*,0,0';
}

// #region Cookie
// Create a cookie with the specified name and value.
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
// Retrieve the value of the cookie with the specified name.
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
/*how to - Check if httponly cookie exists in Javascript
You can indirectly check to see if it exists by trying to set it to a value with javascript if it can't be set, then the HTTP Only Cookie must be there (or the user is blocking cookies).*/
function doesHttpOnlyCookieExist(cookiename) {
    var d = new Date();
    d.setTime(d.getTime() + (1000));
    var expires = "expires=" + d.toUTCString();

    document.cookie = cookiename + "=new_value;path=/;" + expires;
    if (document.cookie.indexOf(cookiename + '=') == -1) {
        return true;
    } else {
        return false;
    }
}
// #endregion
function GestRelease(id) {
    if ($('#' + id).hasClass('success')) {
        console.log('success');
        $('#divListRelShort').hide();
        $('#divListRelComplete').show();
    }
    else if ($('#' + id).hasClass('active')) {
        console.log('chiudi');
        $('#' + id).removeClass();
        $('#' + id).addClass("collapsible");
        $('#' + id).closest('.grouprel').find('.content').css('max-height', '0px');
    }
    else {
        $('#' + id).removeClass();
        $('#' + id).addClass("active");
        //var content = $('#' + id).closest('.grouprel').find('.content');
        //console.log('altezza->' + content.prop('scrollHeight'));
        //content.height = content.prop('scrollHeight')+'px';
        var maxheight = $('#' + id).closest('.grouprel').find('.content').prop('scrollHeight');
        console.log('altoscroll->' + maxheight);
        console.log('alto->' + $('#' + id).closest('.grouprel').find('.content').prop('Height'));
        maxheight = maxheight + 100;
        console.log('alto->' + maxheight);
        $('#' + id).closest('.grouprel').find('.content').css('max-height', maxheight + 'px');
    }
}
function GestModal(id) {
    if ($('#' + id).hasClass('active')) {
        $('#' + id).removeClass();
        $('#' + id).addClass('modal-box');
    }
    else {
        $('#' + id).removeClass();
        $('#' + id).addClass('active');
    }
}
function GestDOCFA(id) {
    if ($('#RicercaDich').is(':visible')) {
        $('#RicercaDich').hide();
        $('#RicercaDOCFA').show();
    }
    GestModal(id);
}