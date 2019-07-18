
const routes = [
  {
    path: '/demo',
    component: () => import('layouts/PaperLayout.vue'),
    children: [
      {
        path: '/demo/:routeName(.*)',
        name: 'demo',
        component: () => import('pages/Demo.vue')
      }
    ]
  },
  {
    path: '/page/:path(.*)*',
    component: () => import('layouts/PaperLayout.vue'),
    name: 'page',
    children: [
      {
        path: '',
        component: () => import('pages/Page.vue')
      }
    ]
  }
]

// Always leave this as last one
if (process.env.MODE !== 'ssr') {
  routes.push({
    path: '*',
    component: () => import('pages/Error404.vue')
  })
}

export default routes
