export function onLocationChanged() {
    window.gtag?.call(this, "event", "page_view", {
        page_location: location.href,
        page_path: location.pathname,
        page_title: document.title
    });
    window._hmt?.push(["_trackPageview", location.pathname]);
}