// after success adjust the page url
// like /c/manage/!<page_no>
$(function () {
    if ($('script[type="x-tmpl-mustache"]') && typeof (paging_options) === "object") {
        $.ajax({
            type: 'post',
            url: paging_options.GetUrl,
            data: { 'pageContentRequest': JSON.stringify(paging_options) },
            dataType: 'json',
            success: function (data) {
                console.log(data);
                paging_options = data;
                renderTableData(data);
                renderPagerData(data);
                setupPager();
            }
        });
    }    
});

function setupPager() {
    $('.page-entry').on('click', function (e) {
        e.preventDefault();
        var pageNo = $(this).data('page-no');
        paging_options.Paging.CurrentPage = pageNo;
        paging_options.Entries = [],
        console.log(paging_options);
        $.ajax({
            type: 'post',
            url: paging_options.GetUrl,
            data: { 'pageContentRequest': JSON.stringify(paging_options) },
            dataType: 'json',
            success: function (data) {
                paging_options = data;
                renderTableData(data);
                renderPagerData(data);
                setupPager();
                window.location.hash = '!/' + pageNo;
            }
        });
    });
}

function renderTableData(options) {
    var template = $('#template').html();
    Mustache.parse(template);
    Mustache.Formatters = {
        date: function (str) {
            var dt = new Date(parseInt(str.substr(6, str.length - 8), 10));
            return (dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear());
        }
    };
    var rendered = Mustache.render(template, {
        Entries: options.Entries
    });
    $('#' + options.Container).html(rendered);
}

function renderPagerData(options) {
    var pagerTemplate = $('#pagerTemplate').html();
    Mustache.parse(pagerTemplate);
    var rendered = Mustache.render(pagerTemplate, {
        FirstPage: 1,
        LastPage: options.Paging.TotalPages,
        Pages: createDataForPager(options)
    });
    $('#pager').html(rendered);
}

function createDataForPager(options) {
    var paging = options.Paging;
    var pages = [];

    for (var i = 1; i <= paging.TotalPages; i++) {
        pages.push({
           PageNumber: i 
        });
    }

    return pages;
}
