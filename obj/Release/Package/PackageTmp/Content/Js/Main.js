$(function () {
    $(window).load(function () {
        if ($(window).height() > $("body").height()) {
            $("#pageEmptyFiller").height($(window).height() - $("body").height() - 105);
        }
    });
    $("[data-slug-source]").each(function () {
        $(this).slugify($(this).attr("data-slug-source"));
    });
    $('.select2-general').select2({
        width: '100%'
    });
    $('.select2-general-full').select2({
        width: '100%'
    });

    $('#dpMain').datepicker({
        todayBtn: 'linked',
        language: 'tr',
        weekStart:1
    }).on('changeDate', function(ev) {
        window.location = "?date=" + moment(ev.date).format("YYYY-MM-DD");
    });
});


tinyMCE.init({
    extended_valid_elements: "a[class|name|href|target|title|onclick|rel],script[type|src|language],iframe[src|style|width|height|scrolling|marginwidth|marginheight|frameborder],img[class|src|border=0|alt|title|hspace|vspace|width|height|align|onmouseover|onmouseout|name]",
    selector: '.tinymce',
    theme: "modern",
    width: "100%",
    valid_elements: '*[*]',
    language: "tr_TR",
    height: 350,
    plugins: [
    "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
    "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
    "save table contextmenu directionality emoticons template paste textcolor"
    ],
    content_css: "/Content/Plugins/bootstrap/css/bootstrap.css",
    toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons",
    file_browser_callback: RoxyFileBrowser
});

function RoxyFileBrowser(field_name, url, type, win) {
    var roxyFileman = '/Content/Plugins/FileMan/index.html';
    if (roxyFileman.indexOf("?") < 0) {
        roxyFileman += "?type=" + type;
    }
    else {
        roxyFileman += "&type=" + type;
    }
    roxyFileman += '&input=' + field_name + '&value=' + document.getElementById(field_name).value;
    tinyMCE.activeEditor.windowManager.open({
        file: roxyFileman,
        title: 'Roxy Fileman',
        width: 850,
        height: 550,
        resizable: "yes",
        plugins: "media",
        inline: "yes",
        close_previous: "no"
    }, { window: win, input: field_name });
    return false;
};