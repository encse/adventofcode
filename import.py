import requests
from bs4 import BeautifulSoup
import sys
import os

url = 'http://adventofcode.com/2017/day/' + sys.argv[1]
if not 'SESSION' in os.environ:
    raise Exception('Cannot find SESSION environment variable')
    
res = requests.get(url, cookies = {'session':os.environ['SESSION']})
content =  res.text
soup = BeautifulSoup(content, 'html.parser')

def unparse_list(sep, tag):
    return sep.join((unparsed for item in tag for unparsed in unparse(item)))

def unparse(tag):
    if tag.name == 'h2':
        yield '## ' + unparse_list('', tag) + '\n'
    elif tag.name == 'p':
        yield  unparse_list('', tag) + '\n'
    elif isinstance(tag, basestring):
        yield  tag
    elif tag.name == 'em':
        yield  '*' + unparse_list('', tag) + '*'
    elif tag.name == 'code':
        if tag.parent.name == 'pre':
            yield unparse_list('', tag)
        else:
            yield  '`'+ unparse_list('', tag) + '`'
    elif tag.name == 'span':
        yield  unparse_list('', tag)
    elif tag.name == 's':
        yield  unparse_list('', tag)
    elif tag.name == 'ul':
        for li in tag:
            for unparsed in unparse (li):
                yield unparsed
    elif tag.name == 'li':
        yield  ' - ' + unparse_list('', tag) 
    elif tag.name == 'pre':
        yield  '```\n' 
        for item in tag:
            for unparsed in unparse (item):
                yield unparsed
        yield  '```\n' 
        
    elif tag.name == 'a':
        yield '['+unparse_list('', tag)+']('+tag.get('href')+')'
    else:
        raise Exception(tag)

print 'original source: [{0}]({0})'.format(url)

for article in soup.findAll("article"):
    print unparse_list('', article)