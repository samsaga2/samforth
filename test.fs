INCLUDE kernel.fs
INCLUDE bios.fs

i: pattern
    create
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c, ;i

pattern star-pattern
0b00000000
0b00000000
0b00000000
0b00001111
0b11110000
0b00000000
0b00000000
0b00000000
0x6f

: char-vaddr ( c -- v-addr )
    3 lshift ;

: char-color-vaddr ( c -- v-addr )
    3 rshift 0x2000 + ;

: bold-font ( -- )
    127 char-vaddr 7 +
    32 char-vaddr
    do
        i RDVRM dup 1 rshift or i WRTVRM
    loop ;

: redefine-char ( char addr -- )
    \ tile
    2dup swap char-vaddr swap 8 -rot LDIRVM
    \ color
    8 + c@ swap char-color-vaddr WRTVRM ;

: show-message
    ." **************************" cr
    ." SamForth " 2009 decimal . cr
    ." by Victor Marzo" cr
    ." <samsaga2@gmail.com>" cr
    ." **************************" cr cr ;

: change-color ( bordercolor backgroundcolor foregroundcolor -- )
    SYS-FORCLR c! SYS-BAKCLR c! SYS-BDRCLR c! CHGCLR ;

: init-screen
    32 SYS-LINL32 c!
    INIT32 0 0 15 change-color
    bold-font
    [char] * ['] star-pattern redefine-char ;

: main
    DISSCR
    init-screen show-message
    ENASCR abort ;
