: init-graphic-mode ( -- )
    INIGRP ;

i: pattern
    create
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c, ;i

i: color-pattern
    create
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c, ;i

\ return the tile pattern address
: char-vaddr ( c -- v-addr )
    3 lshift ;

\ return the tile color address
: char-color-vaddr ( c -- v-addr )
    3 lshift 0x2000 + ;

: ram-to-3banks ( addr vaddr -- )
    2dup swap 8 -rot LDIRVM
    2dup 2048 + swap 8 -rot LDIRVM
    4096 + swap 8 -rot LDIRVM ;

\ copy tile pattern to vram
: redefine-pattern ( addr char -- )
    char-vaddr ram-to-3banks ;

\ copy tile colors to vram
: redefine-color ( addr char -- )
    char-color-vaddr ram-to-3banks ;

\ copy tile pattern and colors to vram
: redefine-tile ( pattern color char -- )
    2dup redefine-color
    nip redefine-pattern ;

: clear-screen ( -- )
    0 0x1800 768 FILVRM ;

: change-color ( bordercolor backgroundcolor foregroundcolor -- )
    SYS-FORCLR c! SYS-BAKCLR c! SYS-BDRCLR c! CHGCLR ;
