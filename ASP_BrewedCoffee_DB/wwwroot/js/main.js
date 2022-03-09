document.addEventListener('DOMContentLoaded', function () {
    //set ajax events on buttons click
    function setButtonsEvents() {
        //ajax-like. анимируем кнопку лайка
        document.querySelectorAll('.post .like-action').forEach((act) => {
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
    };

    setButtonsEvents();

    //lazy load posts   (ON: home page)
    document.querySelectorAll('.posts').forEach((main) => {
        //return;
        if (window.location.pathname != '/') return;
        let page = 1;
        let buzy = false;

        function findControlPost(index){
            let posts = main.querySelectorAll('.post');
            return posts[posts.length - index];
        }
        function isEnd(){
            return this.getBoundingClientRect().top - window.innerHeight <= window.scrollY;
        }
        function getUrl(page){
            return "?page=" + page;
        }
        function getPostsHTML(html) {
            let block = document.createElement('div');
            block.innerHTML = html;
            
            return block.querySelector('.posts');
        }

        function loadPosts() {
            if (buzy) return;
            buzy = true;
            //
            let req = new XMLHttpRequest();
            //
            req.onload = () =>{
                if (req.status != 200) return;
                buzy = false;
                let posts = getPostsHTML(req.responseText);
                main.innerHTML += posts.innerHTML;
                setButtonsEvents();
            }
            //
            req.open('get', getUrl(++page));
            req.send();
        }

        //LoadPosts();
        document.addEventListener('scroll', function (e){
            let post = findControlPost(2);
            if (isEnd.call(post)){
                loadPosts();
            }
        });
    });

    //sidebar mobile animation
    (function () {
        document.querySelectorAll('#hamburger-button').forEach((main) => {
            main.addEventListener('click', function () {
                let scrim = document.querySelector('.scrim');
                scrim.style.display = 'flex';
                //
                let sidebar = document.querySelector('#sidebar');
                let close = sidebar.querySelector('#close-sidebar-button')
                this.style.transition = 'all 300ms';
                this.style.transform = 'scale(0.9)';
                setTimeout(() => {
                    sidebar.style.display = 'block';
                    sidebar.style.marginLeft = '0';
                    close.style.transform = 'scaleX(1.3)';
                }, 100);
                setTimeout(() => {
                    this.style.transform = 'scaleX(1.5)';
                    this.parentNode.style.display = 'none';
                }, 100);
            });
        });
        document.querySelectorAll('#close-sidebar-button').forEach((main) => {
            main.addEventListener('click', function () {
                let scrim = document.querySelector('.scrim');
                let hamburger = document.querySelector('.hamburger-button-wrapper');
                this.style.transition = 'all 300ms';

                setTimeout(() => {
                    this.parentNode.parentNode.style.marginLeft = '-300px';
                    hamburger.style.display = 'inline-block';
                    hamburger.style.transform = 'scale(1)';
                    scrim.style.display = 'none';
                    this.parentNode.parentNode.style.display = 'none';
                }, 100);
            });
        });
        window.matchMedia('(min-width: 800px)').addListener(function (e) {
            if (e.matches) {
                let sidebar = document.querySelector('#sidebar');
                let hamb_wrapper = document.querySelector('.hamburger-button-wrapper');
                hamb_wrapper.style.display = 'none';
                if (!sidebar) return;
                sidebar.style.display = 'block';
                sidebar.style.marginLeft = '0';
            }
        });
        window.matchMedia('(max-width: 800px)').addListener(function (e) {
            if (e.matches) {
                let sidebar = document.querySelector('#sidebar');
                let hamb_wrapper = document.querySelector('.hamburger-button-wrapper');
                if (!document.querySelector('#admin-content')) {
                    hamb_wrapper.style.display = 'inline-block';
                }
                if (!sidebar) return;
                sidebar.style.display = 'none';
                sidebar.style.marginLeft = '-300px';
            }
        });
        document.querySelector('.scrim').addEventListener('click', function (e) {
            let hamburger = document.querySelector('.hamburger-button-wrapper');
            this.style.transition = 'all 300ms';
            let sidebar = document.querySelector('#sidebar');

            setTimeout(() => {
                this.style.display = 'none';
                sidebar.style.marginLeft = '-300px';
                hamburger.style.display = 'inline-block';
                hamburger.style.transform = 'scale(1)';
                sidebar.style.display = 'none';
                e.stopPropagation();
            }, 100);
        });
    })();
    
    //анимируем плавное появление posts
    document.querySelectorAll('.posts').forEach((main) => {
        let p_counter = 0;
        main.style.opacity = 0;
        let posts_opacity_tf = setInterval(() => {
            p_counter += 0.02;
            main.style.opacity = p_counter;
            if (p_counter >= 1)
                clearInterval(posts_opacity_tf);
        }, 1);
    });

    //анимируем плавное появление bg images
    (function(){
        let h_counter = 0;
        let bg_images_opacity_tf = setInterval(() => {
            h_counter += 0.025;
            document.documentElement.style.setProperty('--opacity', h_counter)
            if (h_counter >= 1)
              clearInterval(bg_images_opacity_tf);
        }, 1);
    })();

    //ajax-hidemenu. анимируем сайдбар-менюшки
    document.querySelectorAll('.sidebar-menu-link').forEach((link) => {
        link.addEventListener('click', (e) => {
            let req_show = new XMLHttpRequest();
            let req_hide = new XMLHttpRequest();
            let list = link.parentNode.querySelector('.sidebar-menu-list');
            let menu_title = link.querySelector('h2').innerText;
            let sidebar = document.querySelector('#sidebar');
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
                if (document.body.style.height < sidebar.style.height) {
                    sidebar.style.height = document.body.style.height;
                }
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
            if (!confirm('Are you sure? You can`t restore current data after deleting.'))
                e.preventDefault();
        });
    });
});
