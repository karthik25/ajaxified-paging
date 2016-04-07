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

function renderPagerData(options, currentPageNo) {
    var pagerTemplate = $('#pagerTemplate').html();
    Mustache.parse(pagerTemplate);
    var rendered = Mustache.render(pagerTemplate, {
        FirstPage: 1,
        LastPage: options.Paging.TotalPages,
        Pages: createDataForPager(options, currentPageNo)
    });
    $('#pager').html(rendered);
}

function createDataForPager(options, currentPageNo) {
    var paging = options.Paging;
    var pages = [];

    for (var i = 1; i <= paging.TotalPages; i++) {
        pages.push({
            PageNumber: i,
            PageItemClass: (currentPageNo && i === parseInt(currentPageNo)) ? "active" : ""
        });
    }

    return pages;
}

function pagerCallback(data, pageNo) {
    paging_options = data;
    renderTableData(data);
    renderPagerData(data, pageNo);
    setupPager();
    if (pageNo !== "1") {
        window.location.hash = "!/" + pageNo;
    }
}

function getPagingData(pagingOptions, callback, pageNo) {
    $.ajax({
        type: 'post',
        url: pagingOptions.GetUrl,
        data: { 'pageContentRequest': JSON.stringify(pagingOptions) },
        dataType: 'json',
        success: function (data) {
            callback(data, pageNo);
        }
    });
}

function setupPager() {
    $('.page-entry').on('click', function (e) {
        e.preventDefault();
        var pageNo = $(this).data('page-no');
        paging_options.Paging.CurrentPage = pageNo;
        paging_options.Entries = [],
        getPagingData(paging_options, pagerCallback, pageNo);
    });
}

$(function () {
    if ($('script[type="x-tmpl-mustache"]') && typeof (paging_options) === "object") {
        var pageNo = "1";
        if (window.location.hash) {
            pageNo = window.location.hash.match(/\d+/g)[0];
            paging_options.Paging.CurrentPage = parseInt(pageNo);
        }
        getPagingData(paging_options, pagerCallback, pageNo);
    }
});

$(window).on('hashchange', function () {
    var pageNo = location.hash.slice(1).match(/\d+/g)[0];
    paging_options.Paging.CurrentPage = parseInt(pageNo);
    getPagingData(paging_options, pagerCallback, pageNo);
});
