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

pattern A-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b11111110
0b10000110
0b10000110
0b00000000
pattern B-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b11111110
0b10000110
0b11111110
0b00000000
pattern C-pattern
0b11111100
0b10000110
0b10000000
0b10000000
0b10000000
0b10000110
0b01111110
0b00000000 
pattern D-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b10000110
0b10000110
0b11111110
0b00000000
pattern E-pattern
0b11111100
0b10000000
0b10000000
0b10000000
0b11111110
0b10000000
0b11111110
0b00000000
pattern F-pattern
0b11111100
0b10000000
0b10000000
0b10000000
0b11111110
0b10000000
0b10000000
0b00000000
pattern G-pattern
0b11111100
0b10000110
0b10000000
0b10000000
0b11111100
0b10000110
0b01111110
0b00000000
pattern H-pattern
0b10000100
0b10000110
0b10000110
0b10000110
0b11111110
0b10000110
0b10000110
0b00000000
pattern I-pattern
0b00010000
0b00011000
0b00011000
0b00011000
0b00011000
0b00011000
0b00011000
0b00000000
pattern J-pattern
0b11111000
0b00001100
0b00001100
0b00001100
0b00001100
0b10001100
0b01111100
0b00000000
pattern K-pattern
0b10001000
0b10001100
0b10001100
0b10011000
0b11100000
0b10011100
0b10001100
0b00000000
pattern L-pattern
0b11000000
0b11000000
0b11000000
0b11000000
0b11000000
0b11000000
0b11111110
0b00000000
pattern M-pattern
0b11001100
0b10110110
0b10000110
0b10000110
0b10000110
0b10000110
0b10000110
0b00000000
pattern N-pattern
0b11000110
0b11000110
0b10100110
0b10010110
0b10001110
0b10000110
0b10000110
0b00000000
pattern O-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b10000110
0b10000110
0b01111110
0b00000000
pattern P-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b11111110
0b10000000
0b10000000
0b00000000
pattern Q-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b10010110
0b10001110
0b01111110
0b00000000
pattern R-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b11111110
0b10011100
0b10001100
0b00000000
pattern S-pattern
0b11111000
0b10000000
0b10000000
0b10000000
0b11111100
0b00000110
0b11111110
0b00000000
pattern T-pattern
0b11111110
0b00011000
0b00011000
0b00011000
0b00011000
0b00011000
0b00011000
0b00000000
pattern U-pattern
0b10000100
0b10000110
0b10000110
0b10000110
0b10000110
0b10000110
0b11111110
0b00000000
pattern V-pattern
0b10000100
0b10000110
0b10000110
0b10000110
0b10000110
0b01001110
0b00111100
0b00000000
pattern W-pattern
0b10000100
0b10000110
0b10000110
0b10000110
0b10110110
0b10110110
0b11111110
0b00000000
pattern X-pattern
0b10000100
0b11000110
0b00101100
0b00010000
0b00101100
0b11000110
0b10000110
0b00000000
pattern Y-pattern
0b10000100
0b10000110
0b10000110
0b10000110
0b01111100
0b00011000
0b00011000
0b00000000
pattern Z-pattern
0b11111100
0b00001110
0b00001100
0b00011000
0b01110000
0b10000000
0b11111110
0b00000000

color-pattern char-color
COLOR-LIGHT-RED  COLOR-BLACK
COLOR-DARK-RED   COLOR-BLACK
COLOR-DARK-RED   COLOR-BLACK
COLOR-DARK-RED   COLOR-BLACK
COLOR-LIGHT-RED  COLOR-BLACK
COLOR-MEDIUM-RED COLOR-BLACK
COLOR-MEDIUM-RED COLOR-BLACK
COLOR-MEDIUM-RED COLOR-BLACK

: char-vaddr ( c -- v-addr )
    3 lshift ;

: char-color-vaddr ( c -- v-addr )
    3 lshift 0x2000 + ;

: ram-to-3banks ( addr vaddr -- )
    2dup swap 8 -rot LDIRVM
    2dup 2048 + swap 8 -rot LDIRVM
    4096 + swap 8 -rot LDIRVM ;

: redefine-pattern ( addr char -- )
    char-vaddr ram-to-3banks ;

: redefine-color ( addr char -- )
    char-color-vaddr ram-to-3banks ;

: redefine-tile ( pattern color char -- )
    2dup redefine-color
    nip redefine-pattern ;

: redefine-char ( pattern char -- )
    ['] char-color swap redefine-tile ;

: clear-tiles ( -- )
    0 0x2000 0x2000 FILVRM ;

: redefine-tiles ( -- )
    clear-tiles
    ['] A-pattern [char] A redefine-char
    ['] B-pattern [char] B redefine-char
    ['] C-pattern [char] C redefine-char 
    ['] D-pattern [char] D redefine-char
    ['] E-pattern [char] E redefine-char
    ['] F-pattern [char] F redefine-char
    ['] G-pattern [char] G redefine-char 
    ['] H-pattern [char] H redefine-char
    ['] I-pattern [char] I redefine-char
    ['] J-pattern [char] J redefine-char
    ['] K-pattern [char] K redefine-char 
    ['] L-pattern [char] L redefine-char
    ['] M-pattern [char] M redefine-char
    ['] N-pattern [char] N redefine-char
    ['] O-pattern [char] O redefine-char 
    ['] P-pattern [char] P redefine-char
    ['] Q-pattern [char] Q redefine-char
    ['] R-pattern [char] R redefine-char
    ['] S-pattern [char] S redefine-char
    ['] T-pattern [char] T redefine-char
    ['] U-pattern [char] U redefine-char 
    ['] V-pattern [char] V redefine-char
    ['] W-pattern [char] W redefine-char
    ['] X-pattern [char] X redefine-char
    ['] Y-pattern [char] Y redefine-char
    ['] Z-pattern [char] Z redefine-char ;

: clear-screen
    0 0x1800 768 FILVRM ;

variable cursor

: locate ( x y -- )
    32 * + 0x1800 + cursor ! ;

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

: show-message ( -- )
    0 0 locate
    s" BY" type cr
    s" VICTOR MARZO" type cr cr
    s"     USE LEFT AND RIGHT KEYS" type ;

: change-color ( bordercolor backgroundcolor foregroundcolor -- )
    SYS-FORCLR c! SYS-BAKCLR c! SYS-BDRCLR c! CHGCLR ;

: init-screen ( -- )
    0 0 15 change-color
    INIGRP
    clear-screen
    redefine-tiles ;

: init-sprites ( -- )
    CLRSPR
    \ 16x16 sprites
    SYS-RG1SAV c@ 0b10 or 1 WRTVDP ;

: sprite-name! ( name spriteid -- )
    CALATR 2 + WRTVRM ;

: sprite-color! ( color spriteid -- )
    CALATR 3 + WRTVRM ;

: sprite-pos! ( horizontal vertical spriteid -- )
    CALATR dup 1+ -rot WRTVRM WRTVRM ;

variable sprite-x
variable sprite-y

pattern sprite-pattern
0b00111100
0b00111100
0b00011000
0b11111111
0b00011000
0b00100100
0b00100100
0b01100110

: create-sprite
    8 0 CALPAT ['] sprite-pattern LDIRVM
    0 sprite-x !
    50 8 lshift sprite-y !
    0 0 sprite-name!
    COLOR-WHITE 0 sprite-color! ;

asm: keyboard-status@
    ; ( line -- status )
    di
    in a,(0aah)
    and 0f0h
    add a,c
    out (0aah),a
    ei
    in a,(0a9h)
    ld b,0
    ld c,a
    ei
;asm

: key-status@ ( bitkey -- status )
    keyboard-status@
    swap and
    if 0 else 1 then ;

: KEY-RIGHT 0b10000000 8 ;
: KEY-LEFT  0b00010000 8 ;

: move-sprite
    \ move sprite right
    sprite-x @
    KEY-RIGHT key-status@ if
        50 +
    then
    KEY-LEFT key-status@ if
        50 -
    then
    dup sprite-x !
    \ set sprite pos
    8 rshift sprite-y @ 8 rshift 0 sprite-pos!
    recurse ;

: main
    DISSCR
    init-screen
    init-sprites
    create-sprite
    show-message
    ENASCR
    move-sprite
    abort ;
