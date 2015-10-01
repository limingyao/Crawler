#!/usr/bin/env python
# -*- coding:utf-8 -*-

import cookielib
import urllib2
import bs4
from bs4 import BeautifulSoup

import re
import json
import time
import logging

import sys
reload(sys)
sys.setdefaultencoding('utf8')

log_file = "./app.log"
logging.basicConfig(filename = log_file, level = logging.DEBUG)

def get_doc(page_size = 100, page_num = 1):
    cj = cookielib.CookieJar()
    opener = urllib2.build_opener(urllib2.HTTPCookieProcessor(cj))
    
    url = 'http://scjg.xn121.com/search.php?area=&name=&one_type=0&two_type=0&num=%d&page=%d' % (page_size, page_num)
    logging.info(url)
    
    request = urllib2.Request(url)
    response = opener.open(request)
    doc = response.read()
    response.close()
    return doc


def parser(doc):
    soup = BeautifulSoup(doc, "html.parser")
    trs = soup.find_all(name = 'tr', attrs = {'onmouseover' : 'this.className=\'active\''})
    ret_list = []
    if len(trs) < 1:
        return ret_list
    for tr in trs:
        tds = tr.find_all('td')
        if len(tds) < 4:
            logging.error('parser error ...')
        
        name = tds[0].text
        price = tds[1].text
        organization = tds[2].text
        date =  tds[3].text
        
        data = {}
        data['name'] = name
        data['price'] = price
        data['organization'] = organization
        data['date'] = date
    
        ret_list.append(json.dumps(data))
    return ret_list

i = 1
retry_all = 5
page_size = 10000
file_name = 'data.json'
file = open(file_name, "w")

while True:
    retry = retry_all
    while retry > 0:
        try:
            doc = get_doc(page_size, i)
            rets = parser(doc)
            if len(rets) > 0:
                for ret in rets:
                    file.write(ret + "\n")
                break
        except Exception as ex:
            logging.error('request error, retry ...')
        finally:
            retry -= 1
    file.flush()
    if retry < 1:
        logging.info('crawler ending ...')
        break
    logging.info('sleep 30s ...')
    time.sleep(30)
    i += 1

file.close()
