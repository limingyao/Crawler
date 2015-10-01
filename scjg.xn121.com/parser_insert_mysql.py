#!/usr/bin/env python
# -*- coding:utf-8 -*-

import cookielib
import urllib2
import bs4
from bs4 import BeautifulSoup

import re
import json
import time
import MySQLdb

import sys
reload(sys)
sys.setdefaultencoding('utf8')

host_ = '127.0.0.1'
user_ = 'root'
passwd_ = 'sa'
db_ = 'grain'
port_ = 3306


def insert(name, price, organization, date):
    sql = "insert into grain(grain_name, grain_price, grain_organization, grain_date) values('%s', '%s', '%s', '%s')"
    retry = 3
    while (retry > 0):
        try:
            conn = MySQLdb.connect(host = host_, user = user_, passwd = passwd_, db = db_, port = port_)
            conn.set_character_set('utf8')
            
            cur = conn.cursor()
            cur.execute('SET NAMES utf8;') 
            cur.execute('SET CHARACTER SET utf8;')
            cur.execute('SET character_set_connection=utf8;')
            
            ret = cur.execute(sql % (name, price, organization, date))
            conn.commit()
            cur.close()
            conn.close()
            return ret
        except MySQLdb.Error as e:
            retry -= 1
            print "[%s] Mysql Error %d: %s" % ('insert', e.args[0], e.args[1])
    return 0


if __name__ == "__main__":
    file_name = 'data.json'
    file = open(file_name)
    while True:
        line = file.readline()
        if not line:
            break
        line = line.strip()
        name = json.loads(line)['name']
        price = json.loads(line)['price']
        organization = json.loads(line)['organization']
        date = json.loads(line)['date']
        ret = insert(name, price, organization, date)
        if ret > 0:
            print 'Insert Success.'
