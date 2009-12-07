hex

\ system vars
f3ae const linl40
f3af const linl32
f3e9 const forclr
f3ea const bakclr
f3eb const bdrclr

: disable-screen ( -- )
    0 0 0 0 0041 SYSCALL 4drop ;

: enable-screen ( -- )
    0 0 0 0 0044 SYSCALL 4drop ;

: vdp! ( data register -- )
    8 lshift + >r
    0 r> 0 0 0047 SYSCALL 4drop ;

: vram@ ( address -- data )
    >r 0 0 0 r> 004a SYSCALL 3drop ;

: vram! ( data address -- )
    >r 0 0 r> 004d SYSCALL 4drop ;

: fill-vram ( data length address -- )
    0 swap 0056 SYSCALL 4drop ;

: vram-to-ram ( length destination source -- )
    >r >r >r 0 r> r> r> 0059 SYSCALL 4drop ;

: ram-to-vram ( length destination source -- )
    >r >r >r 0 r> r> r> 005c SYSCALL 4drop ;

: clear-sprites ( -- )
    0 0 0 0 0069 SYSCALL 4drop ;

: cursor-position ( column row -- )
    8 lshift + >r
    0 0 0 r> 00c6 SYSCALL 4drop ;

: change-color ( bordercolor backgroundcolor foregroundcolor -- )
    forclr c! bakclr c! bdrclr c!
    0 0 0 0 0062 SYSCALL 4drop ;

: init-text ( -- )
    0 0 0 0 006c SYSCALL 4drop ;

: init-mode32 ( -- )
    0 0 0 0 006f SYSCALL 4drop ;

: init-graph ( -- )
    0 0 0 0 0072 SYSCALL 4drop ;

: init-multi ( -- )
    0 0 0 0 0075 SYSCALL 4drop ;

: sprite-pattern ( spriteid -- address )
    0 0 0 0084 SYSCALL nip nip nip ;

: sprite-attribute ( spriteid -- address )
    0 0 0 0087 SYSCALL nip nip nip ;

: sprite-size ( -- size )
    0 0 0 0 008a SYSCALL 3drop ;

decimal
