document.addEventListener('DOMContentLoaded', function () {

    (function(){
        let posts = document.querySelector('.posts');
        let counter = 0;
        posts.style.opacity = "0%";
        let opacity_tf = setInterval(() => {
            counter += 3;
            posts.style.opacity = counter + "%";
            if (posts.style.opacity >= 100)
              clearInterval(opacity_tf);
        }, 1);
    })();

    //ajax-like. анимируем кнопку лайка
    document.querySelectorAll('.post .like-action').forEach((act, i) => {
        let req_like = new XMLHttpRequest();
        let req_dislike = new XMLHttpRequest();
        let btn = act.querySelector('.btn-likes-action')

        // реагистрация обработчиков после отправки
        req_like.onload = () => {
            if (req_like.status != 200) return;

            btn.classList.remove('btn-like');
            btn.classList.add('btn-like-pressed');
            let count_likes = btn.querySelector('.count-likes');
            if (!count_likes) {
                let cl = document.createElement('span');
                cl.className = 'count-likes';
                btn.prepend(cl);
                count_likes = cl;
            }
            count_likes.style.display = 'inline-block';
            count_likes.innerText = req_like.responseText;
            btn.querySelector('.btn-icon').classList.add('btn-icon-like-pressed');
            btn.setAttribute('like-action', 'dislike');
        };
        req_dislike.onload = () => {
            if (req_dislike.status != 200) return;
            btn.classList.remove('btn-like-pressed');
            btn.classList.add('btn-like');
            let count_likes = btn.querySelector('.count-likes');
            if (count_likes) count_likes.style.display = 'none';
            btn.querySelector('.btn-icon').classList.remove('btn-icon-like-pressed');
            btn.setAttribute('like-action', 'like');
        };

        // Привязываем отправку лайка \ дизлайка
        btn.addEventListener('click', () => {
            btn.style.transform = 'scale(1.1)';
            setTimeout(() => {
                btn.style.transform = 'scale(1)';
            }, 100);
            let acttype = btn.getAttribute('like-action');
            let postid = act.getAttribute('post-id');
            let req = acttype == 'like' ? req_like : req_dislike;
            req.open('post', `/ajax-like/?postid=${postid}&acttype=${acttype}`);
            req.send();
        });
    });

    //ajax-favorite. анимируем кнопку избранное
    document.querySelectorAll('.post .favorite-action').forEach((act, i) => {
        let req_to_fav = new XMLHttpRequest();
        let req_from_fav = new XMLHttpRequest();
        let btn = act.querySelector('.favorite-btn')

        // реагистрация обработчиков после отправки
        req_to_fav.onload = () => {
            if (req_to_fav.status != 200) return;
            btn.innerText = 'favorite';
            btn.classList.add('in-favorites');
            btn.setAttribute('fav-action', 'from-fav')
        };
        req_from_fav.onload = () => {
            if (req_from_fav.status != 200) return;
            btn.innerText = 'to favorites';
            btn.classList.remove('in-favorites');
            btn.setAttribute('fav-action', 'to-fav')
        };

        // Привязываем отправку
        btn.addEventListener('click', () => {
            btn.style.transform = 'scale(0.9)';
            btn.style.transition = 'all 0ms';
            setTimeout(() => {
                btn.style.transition = 'all 200ms';
                btn.style.transform = 'scale(1)';
            }, 100);

            let acttype = btn.getAttribute('fav-action');
            let postid = act.getAttribute('post-id');
            let req = acttype == 'to-fav' ? req_to_fav : req_from_fav;
            req.open('post', `/ajax-favorite/?postid=${postid}`);
            req.send();
        });
    });

    //ajax-hidemenu. анимируем сайдбар-менюшки
    document.querySelectorAll('.sidebar-menu-link').forEach((link) => {
        link.addEventListener('click', (e) => {
            let req_show = new XMLHttpRequest();
            let req_hide = new XMLHttpRequest();
            let list = link.parentNode.querySelector('.sidebar-menu-list');
            let menu_title = link.querySelector('h2').innerText;

            // реагистрация обработчиков после отправки
            req_show.onload = () => {
                if (req.status != 200) return;
                list.classList.remove('hidden-list');
                list.setAttribute('hide-action', 'hide')
            };
            req_hide.onload = () => {
                if (req.status != 200) return;
                list.classList.add('hidden-list');
                list.setAttribute('hide-action', 'show')
            };

            // Привязываем отправку
            let acttype = list.getAttribute('hide-action');
            let req = acttype == 'show' ? req_show : req_hide;
            req.open('post', `/ajax-hidemenu/?title=${menu_title}`);
            req.send();

            e.preventDefault();
        });
    });

    //confirm delete admin-list-item
    document.querySelectorAll('.admin-list-item .del').forEach((link) => {
        link.addEventListener('click', (e) => {
            if (!confirm('Are you sure ? You can`t restore this data after deleting.'))
                e.preventDefault();
        });
    });
});
