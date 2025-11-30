import re
import glob

# Pattern to find and fix the misplaced buttons
def fix_page(filename):
    with open(filename, 'r', encoding='utf-8') as f:
        content = f.read()
    
    original = content
    
    # Fix: move closing </div> after page-number and before PDF buttons
    pattern = r'(<span class="page-number">.*?</span>)\s*(<!-- أزرار تصدير PDF -->.*?<div class="pdf-buttons">.*?</div>)\s*</div>'
    replacement = r'\1</div>\n\n        \2'
    
    content = re.sub(pattern, replacement, content, flags=re.DOTALL)
    
    if content != original:
        with open(filename, 'w', encoding='utf-8', newline='') as f:
            f.write(content)
        return True
    return False

# Fix all pages
fixed = 0
for page in glob.glob('page-*.html'):
    if fix_page(page):
        print(f'Fixed: {page}')
        fixed += 1

print(f'\nTotal fixed: {fixed} pages')
