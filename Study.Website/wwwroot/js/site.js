// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function repositionToolbar() {
    // 根据 summernote className 找到他的 toolbar
    const $editor = $('.summernote').siblings('.note-editor');
    const $toolbar = $editor.find('.note-toolbar');



    // 得到浏览器当前滚动条 top 位置(绝对)
    const windowTop = $(window).scrollTop();


    // 编辑器的 top 位置(绝对坐标)
    const editorTop = $editor.offset().top;


    // console.log('滚动条顶部' + windowTop);
    //console.log('编辑器顶部' + editorTop);
    $toolbar.css('width', $editor.width().toString() + 'px');

    if (/*editorTop - windowTop > 200 ||*/ windowTop > editorTop) {
        $toolbar.css('position', 'fixed');
        $toolbar.css('width', $editor.width().toString() + 'px');
        //$toolbar.css('top', '50px');
        $toolbar.css('top', '60px')
        $toolbar.css('background-color', 'white')
        //$toolbar.css('z-index', '9999966');
    } else {
        $toolbar.css('position', 'static');
        $editor.css('padding-top', '0px');
    }
}

/**
 * 上传文件到服务器
 * @param {any} url
 * @param {any} data
 * @param {any} cb   function (axiosres){}
 */
function uploadToServer(url, data, cb) {
    //上传图片到服务器
    var formData = new FormData();
    formData.append('file', data);
    axios.post(url, formData, {
        method: 'post',
        headers: {
            'Content-Type': 'multipart/form-data'
        },
        transformRequest: [function (data) {
            return data;
        }],
        onUploadProgress: function (e) {
            var v = ((e.loaded * 100) / e.total) || 0;
            var percentage = Math.round(v);
            if (percentage < 100) {
                swal('上传进度' + percentage + '%');
            }
        }
    }).then(function (resp) {
        cb(resp);
    }).catch(function (error) {
        console.log(error.response);
        swal("上传到服务器发生了错误,http状态码【" + error.response.status + '】', error.response.data, "error");
    });
}

/**
 * 压缩文件
 * @param {any} file  文件
 * @param {any} successCb   成功回调 function (file){}
 */
function CompressorFile(rawFile, successCb) {
    var options = {
        strict: true,
        checkOrientation: true,
        maxWidth: 800,
        maxHeight: undefined,
        minWidth: 0,
        minHeight: 0,
        width: undefined,
        height: undefined,
        quality: 0.6,
        mimeType: '',
        convertSize: 5000000,
        success: function (file) {
            var preSize = rawFile.size;
            var nowSize = file.size;

            //压缩后小于压缩前
            if (nowSize < preSize) {
                successCb(file);
            } else {
                successCb(rawFile);
            }
        },
        error: function (errmessage) {
            swal('错误', errmessage.message, "error");
        }
    };

    new Compressor(rawFile, options);
}

function initSummernote(selector) {
    $(window).scroll(function () {
        //console.log('滚动');
        repositionToolbar();
    });


    $(window).resize(function () {
        this.console.log('大小改动');
        repositionToolbar();
    });


    $(selector).summernote({
        disableDragAndDrop: true,
        lang: 'zh-CN', // default: 'en-US'
        placeholder: "1、不允许拖拽文件；2、工具栏<i class='note-icon-picture'></i>图片上传，图片最大50MB；3、编辑器可自由拉长",
        callbacks: {
            onImageUpload: function (files) {
                var size = files[0].size / 1024 / 1024;
                if (size > 50) {
                    //Math.round四舍五入
                    swal('错误', "上传的图片不能超过50M，您当前的图片是 " + Math.round(size) + "MB", "error");
                    return false;
                }

                CompressorFile(files[0],
                    function (result) {
                        uploadToServer('/api/file', result, function (res) {
                            $('.summernote').summernote('insertImage', res.data.link);
                            swal("成功", '上传了图片,图片大小 ' + Math.round(result.size / 1024) + 'KB', 'success');
                        });
                    });

            }
        }
    });
}

