document.addEventListener('DOMContentLoaded', function () {
    let Offset;
    // убираем якорь на кнопках в скетче поста
    /*document.querySelectorAll('.post form').forEach((form, i) => {
        form.addEventListener('submit', function () {
            form.submit();
            window.scrollTo(0, Offset);
        });
    });
    document.querySelectorAll('.post').forEach((post, i) => {
        post.querySelectorAll('.btn, .to-favorites').forEach((button, i) => {
            button.addEventListener('click', function () {
                Offset = window.pageYOffset;
            });
        });
    });*/
    /*document.querySelectorAll('.post form').forEach((frm, i) => {
        frm.addEventListener('submit', function (ev) {
            $.ajax({
                type: frm.attr('method'),
                url: frm.attr('action'),
                data: frm.serialize(),
                success: function (data) {
                    alert('ok');
                }
            });
            ev.preventDefault();
        });

        function submitForm2() {
            frm.submit();
        }
    });*/
   /* $(document).ready(function () {
        var frm = $('form');
        frm.submit(function (ev) {
            $.ajax({
                type: frm.attr('method'),
                url: frm.attr('action'),
                data: frm.serialize(),
            });
            ev.preventDefault();
        });
    });

    function submitForm2() {
        $('form').submit();
    }*/
});
