\ initialize graphics
: init-graphic-mode ( -- )
    INIGRP ;

\ parse 8x8 pattern
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

\ parse 2x8 pattern color
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

\ copy 8x8 pattern to 3 banks vram
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

\ fill screen with 0
: clear-screen ( -- )
    0 0x1800 768 FILVRM ;

\ change screen colors
: change-color ( bordercolor backgroundcolor foregroundcolor -- )
    SYS-FORCLR c! SYS-BAKCLR c! SYS-BDRCLR c! CHGCLR ;

\ screen vram address
: base-screen ( -- vaddr )
    0x1800 ;

: locate-addr ( x y -- addr )
    32 * + ;

: locate-vaddr ( x y -- vaddr )
    locate-addr base-screen + ;

\ write to vram
: vram! ( data vaddr -- )
    WRTVRM ;

\ read from vram
: vram@ ( vaddr -- n )
    RDVRM ;

variable cursor

: locate ( x y -- )
    locate-vaddr cursor ! ;

: 1+! ( addr -- )
    dup @ 1+ swap ! ;

: emit ( n -- )
    cursor @ WRTVRM 
    cursor 1+! ;

: type ( c-addr +n -- )
    ?dup if
        over + swap do i c@ emit loop
    else drop then ;

: cr ( -- )
    cursor @ 0b1111111111100000 and 32 + cursor ! ;

: space ( -- )
    32 emit ;
