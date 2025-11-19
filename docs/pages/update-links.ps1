# Script to update page navigation links
$pages = @{
    "page-06.html" = @{ num="6"; prev="page-05.html"; next="page-07.html" }
    "page-07.html" = @{ num="7"; prev="page-06.html"; next="page-08.html" }
    "page-08.html" = @{ num="8"; prev="page-07.html"; next="page-09.html" }
    "page-09.html" = @{ num="9"; prev="page-08.html"; next="page-10.html" }
    "page-10.html" = @{ num="10"; prev="page-09.html"; next="page-11.html" }
    "page-11.html" = @{ num="11"; prev="page-10.html"; next="page-12.html" }
    "page-12.html" = @{ num="12"; prev="page-11.html"; next="page-13.html" }
    "page-13.html" = @{ num="13"; prev="page-12.html"; next="page-14.html" }
    "page-14.html" = @{ num="14"; prev="page-13.html"; next="page-15.html" }
    "page-15.html" = @{ num="15"; prev="page-14.html"; next="page-16.html" }
    "page-16.html" = @{ num="16"; prev="page-15.html"; next="page-17.html" }
    "page-17.html" = @{ num="17"; prev="page-16.html"; next="page-18.html" }
    "page-18.html" = @{ num="18"; prev="page-17.html"; next="page-19.html" }
    "page-19.html" = @{ num="19"; prev="page-18.html"; next="page-20.html" }
    "page-20.html" = @{ num="20"; prev="page-19.html"; next="page-21.html" }
    "page-21.html" = @{ num="21"; prev="page-20.html"; next="page-22.html" }
    "page-22.html" = @{ num="22"; prev="page-21.html"; next="index.html" }
}

foreach($page in $pages.Keys) {
    $file = "d:\Work\projects\Sahl\Project\docs\pages\$page"
    if(Test-Path $file) {
        $content = Get-Content $file -Raw -Encoding UTF8
        $num = $pages[$page].num
        $prev = $pages[$page].prev
        $next = $pages[$page].next
        
        # Update page number
        $content = $content -replace 'صفحة \d+ من 22', "صفحة $num من 22"
        
        # Update prev link
        $content = $content -replace 'href="page-\d+\.html" class="btn-nav">⬅️', "href=`"$prev`" class=`"btn-nav`">⬅️"
        
        # Update next link
        $content = $content -replace 'href="page-\d+\.html" class="btn-nav btn-next">[^<]+', "href=`"$next`" class=`"btn-nav btn-next`">الصفحة التالية"
        
        Set-Content $file $content -Encoding UTF8 -NoNewline
        Write-Host "Updated $page"
    }
}
