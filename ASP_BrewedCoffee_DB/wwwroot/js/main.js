document.addEventListener('DOMContentLoaded', function () {
    //ajax-like
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
            let req = acttype == "like" ? req_like : req_dislike;
            req.open('get', `/ajax-like/?postid=${postid}&acttype=${acttype}`);
            req.send();
        });
    });

    //ajax-favorite
    document.querySelectorAll('.post .favorite-action').forEach((act, i) => {
        let req_to_fav = new XMLHttpRequest();
        let req_from_fav = new XMLHttpRequest();
        let btn = act.querySelector('.favorite-btn')
        //let is_in_fav = false;
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
            let req = acttype == "to-fav" ? req_to_fav : req_from_fav;
            req.open('get', `/ajax-favorite/?postid=${postid}`);
            req.send();
        });
    });

    /*(function () {
        let req = new XMLHttpRequest();
        
        // реагистрация обработчикa после отправки 
        req.onload = () => {
            if (req.status != 200) return;
            let data = JSON.parse(req.responseText);

            document.querySelectorAll('.sidebar-menu-link').forEach((link) => {
                let menu_title = link.querySelector('h2').innerText;
                if (data.includes(menu_title)) {
                    let list = link.parentNode.querySelector('.sidebar-menu-list');
                    list.style.height = '0px';
                    list.style.opacity = '0';
                }
            });
        };

        // Привязываем отправку
        
        req.open('get', '/ajax-session/?key=sidebar-menu-titles');
        req.send();
        
    })();*/


    //
    /*function getRekt(link, num_ms) {
        let list = link.parentNode.querySelector('.sidebar-menu-list');
        list.style.transition = `all ${num_ms}ms`;
        list.style.overflow = 'hidden';

        if (list.style.height == '0px') {
            list.style.opacity = '100';
            list.style.height = list.getAttribute('ht-offset') + 'px';
        }
        else {
            list.style.height = list.offsetHeight + 'px';
            list.setAttribute('ht-offset', list.offsetHeight);
            let ht = list.offsetHeight;
            list.style.opacity = '0';
            list.style.height = '0px';
        }
    }*/
    // анимируем сайдбар-менюшки
    document.querySelectorAll('.sidebar-menu-link').forEach((link) => {
        link.addEventListener('click', (e) => {
            let req_show = new XMLHttpRequest();
            let req_hide = new XMLHttpRequest();
            let list = link.parentNode.querySelector('.sidebar-menu-list');
            let menu_title = link.querySelector('h2').innerText;

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
            let req = acttype == "show" ? req_show : req_hide;
            req.open('get', `/ajax-hide/?title=${menu_title}`);
            req.send();

            e.preventDefault();
        });
    });
});
