﻿function signatureCapture() {
    var canvas = document.getElementById("newSignature");
    var context = canvas.getContext("2d");
    canvas.width = 276;
    canvas.height = 180;
    context.fillStyle = "#fff";
    context.strokeStyle = "#444";
    context.lineWidth = 1.5;
    context.lineCap = "round";
    context.fillRect(0, 0, canvas.width, canvas.height);
    var disableSave = true;
    var pixels = [];
    var cpixels = [];
    var xyLast = {};
    var xyAddLast = {};
    var calculate = false;
    {   //functions
        function remove_event_listeners() {
            canvas.removeEventListener('mousemove', on_mousemove, false);
            canvas.removeEventListener('mouseup', on_mouseup, false);
            canvas.removeEventListener('touchmove', on_mousemove, false);
            canvas.removeEventListener('touchend', on_mouseup, false);

            document.body.removeEventListener('mouseup', on_mouseup, false);
            document.body.removeEventListener('touchend', on_mouseup, false);
        }

        function get_coords(e) {
            var x, y;

            if (e.changedTouches && e.changedTouches[0]) {
                var offsety = canvas.offsetTop || 0;
                var offsetx = canvas.offsetLeft || 0;

                x = e.changedTouches[0].pageX - offsetx;
                y = e.changedTouches[0].pageY - offsety;
            } else if (e.layerX || 0 == e.layerX) {
                x = e.layerX;
                y = e.layerY;
            } else if (e.offsetX || 0 == e.offsetX) {
                x = e.offsetX;
                y = e.offsetY;
            }

            return {
                x: x, y: y
            };
        };

        function on_mousedown(e) {
            e.preventDefault();
            e.stopPropagation();

            canvas.addEventListener('mouseup', on_mouseup, false);
            canvas.addEventListener('mousemove', on_mousemove, false);
            canvas.addEventListener('touchend', on_mouseup, false);
            canvas.addEventListener('touchmove', on_mousemove, false);
            document.body.addEventListener('mouseup', on_mouseup, false);
            document.body.addEventListener('touchend', on_mouseup, false);

            empty = false;
            var xy = get_coords(e);
            context.beginPath();
            pixels.push('moveStart');
            context.moveTo(xy.x, xy.y);
            pixels.push(xy.x, xy.y);
            xyLast = xy;
        };

        function on_mousemove(e, finish) {
            e.preventDefault();
            e.stopPropagation();

            var xy = get_coords(e);
            var xyAdd = {
                x: (xyLast.x + xy.x) / 2,
                y: (xyLast.y + xy.y) / 2
            };

            if (calculate) {
                var xLast = (xyAddLast.x + xyLast.x + xyAdd.x) / 3;
                var yLast = (xyAddLast.y + xyLast.y + xyAdd.y) / 3;
                pixels.push(xLast, yLast);
            } else {
                calculate = true;
            }

            context.quadraticCurveTo(xyLast.x, xyLast.y, xyAdd.x, xyAdd.y);
            pixels.push(xyAdd.x, xyAdd.y);
            context.stroke();
            context.beginPath();
            context.moveTo(xyAdd.x, xyAdd.y);
            xyAddLast = xyAdd;
            xyLast = xy;

        };

        function on_mouseup(e) {
            remove_event_listeners();
            disableSave = false;
            context.stroke();
            pixels.push('e');
            calculate = false;
        };
    }
    canvas.addEventListener('touchstart', on_mousedown, false);
    canvas.addEventListener('mousedown', on_mousedown, false);
}

function signatureSave() {
    var blank = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAARQAAAC0CAYAAABVG9MjAAAAAXNSR0IArs4c6QAABPZJREFUeF7t1LENADAMw7D2/6NdoDdoZA7wQAS623YcAQIEAoErKIGiCQIEvoCgeAQCBDIBQckoDREgICh+gACBTEBQMkpDBAgIih8gQCATEJSM0hABAoLiBwgQyAQEJaM0RICAoPgBAgQyAUHJKA0RICAofoAAgUxAUDJKQwQICIofIEAgExCUjNIQAQKC4gcIEMgEBCWjNESAgKD4AQIEMgFBySgNESAgKH6AAIFMQFAySkMECAiKHyBAIBMQlIzSEAECguIHCBDIBAQlozREgICg+AECBDIBQckoDREgICh+gACBTEBQMkpDBAgIih8gQCATEJSM0hABAoLiBwgQyAQEJaM0RICAoPgBAgQyAUHJKA0RICAofoAAgUxAUDJKQwQICIofIEAgExCUjNIQAQKC4gcIEMgEBCWjNESAgKD4AQIEMgFBySgNESAgKH6AAIFMQFAySkMECAiKHyBAIBMQlIzSEAECguIHCBDIBAQlozREgICg+AECBDIBQckoDREgICh+gACBTEBQMkpDBAgIih8gQCATEJSM0hABAoLiBwgQyAQEJaM0RICAoPgBAgQyAUHJKA0RICAofoAAgUxAUDJKQwQICIofIEAgExCUjNIQAQKC4gcIEMgEBCWjNESAgKD4AQIEMgFBySgNESAgKH6AAIFMQFAySkMECAiKHyBAIBMQlIzSEAECguIHCBDIBAQlozREgICg+AECBDIBQckoDREgICh+gACBTEBQMkpDBAgIih8gQCATEJSM0hABAoLiBwgQyAQEJaM0RICAoPgBAgQyAUHJKA0RICAofoAAgUxAUDJKQwQICIofIEAgExCUjNIQAQKC4gcIEMgEBCWjNESAgKD4AQIEMgFBySgNESAgKH6AAIFMQFAySkMECAiKHyBAIBMQlIzSEAECguIHCBDIBAQlozREgICg+AECBDIBQckoDREgICh+gACBTEBQMkpDBAgIih8gQCATEJSM0hABAoLiBwgQyAQEJaM0RICAoPgBAgQyAUHJKA0RICAofoAAgUxAUDJKQwQICIofIEAgExCUjNIQAQKC4gcIEMgEBCWjNESAgKD4AQIEMgFBySgNESAgKH6AAIFMQFAySkMECAiKHyBAIBMQlIzSEAECguIHCBDIBAQlozREgICg+AECBDIBQckoDREgICh+gACBTEBQMkpDBAgIih8gQCATEJSM0hABAoLiBwgQyAQEJaM0RICAoPgBAgQyAUHJKA0RICAofoAAgUxAUDJKQwQICIofIEAgExCUjNIQAQKC4gcIEMgEBCWjNESAgKD4AQIEMgFBySgNESAgKH6AAIFMQFAySkMECAiKHyBAIBMQlIzSEAECguIHCBDIBAQlozREgICg+AECBDIBQckoDREgICh+gACBTEBQMkpDBAgIih8gQCATEJSM0hABAoLiBwgQyAQEJaM0RICAoPgBAgQyAUHJKA0RICAofoAAgUxAUDJKQwQICIofIEAgExCUjNIQAQKC4gcIEMgEBCWjNESAgKD4AQIEMgFBySgNESAgKH6AAIFMQFAySkMECAiKHyBAIBMQlIzSEAECguIHCBDIBAQlozREgICg+AECBDIBQckoDREgICh+gACBTEBQMkpDBAgIih8gQCATEJSM0hABAoLiBwgQyAQEJaM0RICAoPgBAgQyAUHJKA0RIPAARFnOAwdMoEkAAAAASUVORK5CYII=";
    var blank2 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAARQAAAC0CAYAAABVG9MjAAAAAXNSR0IArs4c6QAABPBJREFUeF7t1LENAAAIwzD6/9M8kdEc0MFC2TkCBAhEAot2zBAgQOAExRMQIJAJCEpGaYgAAUHxAwQIZAKCklEaIkBAUPwAAQKZgKBklIYIEBAUP0CAQCYgKBmlIQIEBMUPECCQCQhKRmmIAAFB8QMECGQCgpJRGiJAQFD8AAECmYCgZJSGCBAQFD9AgEAmICgZpSECBATFDxAgkAkISkZpiAABQfEDBAhkAoKSURoiQEBQ/AABApmAoGSUhggQEBQ/QIBAJiAoGaUhAgQExQ8QIJAJCEpGaYgAAUHxAwQIZAKCklEaIkBAUPwAAQKZgKBklIYIEBAUP0CAQCYgKBmlIQIEBMUPECCQCQhKRmmIAAFB8QMECGQCgpJRGiJAQFD8AAECmYCgZJSGCBAQFD9AgEAmICgZpSECBATFDxAgkAkISkZpiAABQfEDBAhkAoKSURoiQEBQ/AABApmAoGSUhggQEBQ/QIBAJiAoGaUhAgQExQ8QIJAJCEpGaYgAAUHxAwQIZAKCklEaIkBAUPwAAQKZgKBklIYIEBAUP0CAQCYgKBmlIQIEBMUPECCQCQhKRmmIAAFB8QMECGQCgpJRGiJAQFD8AAECmYCgZJSGCBAQFD9AgEAmICgZpSECBATFDxAgkAkISkZpiAABQfEDBAhkAoKSURoiQEBQ/AABApmAoGSUhggQEBQ/QIBAJiAoGaUhAgQExQ8QIJAJCEpGaYgAAUHxAwQIZAKCklEaIkBAUPwAAQKZgKBklIYIEBAUP0CAQCYgKBmlIQIEBMUPECCQCQhKRmmIAAFB8QMECGQCgpJRGiJAQFD8AAECmYCgZJSGCBAQFD9AgEAmICgZpSECBATFDxAgkAkISkZpiAABQfEDBAhkAoKSURoiQEBQ/AABApmAoGSUhggQEBQ/QIBAJiAoGaUhAgQExQ8QIJAJCEpGaYgAAUHxAwQIZAKCklEaIkBAUPwAAQKZgKBklIYIEBAUP0CAQCYgKBmlIQIEBMUPECCQCQhKRmmIAAFB8QMECGQCgpJRGiJAQFD8AAECmYCgZJSGCBAQFD9AgEAmICgZpSECBATFDxAgkAkISkZpiAABQfEDBAhkAoKSURoiQEBQ/AABApmAoGSUhggQEBQ/QIBAJiAoGaUhAgQExQ8QIJAJCEpGaYgAAUHxAwQIZAKCklEaIkBAUPwAAQKZgKBklIYIEBAUP0CAQCYgKBmlIQIEBMUPECCQCQhKRmmIAAFB8QMECGQCgpJRGiJAQFD8AAECmYCgZJSGCBAQFD9AgEAmICgZpSECBATFDxAgkAkISkZpiAABQfEDBAhkAoKSURoiQEBQ/AABApmAoGSUhggQEBQ/QIBAJiAoGaUhAgQExQ8QIJAJCEpGaYgAAUHxAwQIZAKCklEaIkBAUPwAAQKZgKBklIYIEBAUP0CAQCYgKBmlIQIEBMUPECCQCQhKRmmIAAFB8QMECGQCgpJRGiJAQFD8AAECmYCgZJSGCBAQFD9AgEAmICgZpSECBATFDxAgkAkISkZpiAABQfEDBAhkAoKSURoiQEBQ/AABApmAoGSUhggQEBQ/QIBAJiAoGaUhAgQExQ8QIJAJCEpGaYgAAUHxAwQIZAKCklEaIkBAUPwAAQKZgKBklIYIEBAUP0CAQCYgKBmlIQIEBMUPECCQCQhKRmmIAIEHw3EAtbJJCLYAAAAASUVORK5CYII=";
    var canvas = document.getElementById("newSignature");// save canvas image as data url (png format by default)
    var dataURL = canvas.toDataURL("image/png");
    if (dataURL === blank || dataURL === blank2) {
        $("#blankMessage").show({ duration: "fast" })
        $("#imgDiv").hide({ duration: "fast" })
    }
    else {
        document.getElementById("saveSignature").src = dataURL;
        $("#imgDiv").show({ duration: "fast" })
        $("#blankMessage").hide({ duration: "fast" })
        console.log(dataURL);
        $("#dataUrl").val(dataURL);
    }
};

function signatureClear() {
    var canvas = document.getElementById("newSignature");
    var context = canvas.getContext("2d");
    context.clearRect(0, 0, canvas.width, canvas.height);
}
