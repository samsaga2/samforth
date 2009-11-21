HEX

\ System vars
F3AE CONST LINL40
F3AF CONST LINL32
F3E9 CONST FORCLR
F3EA CONST BAKCLR
F3EB CONST BDRCLR

: DISABLE-SCREEN ( -- )
    0 0 0 0 0041 SYSCALL 4DROP ;

: ENABLE-SCREEN ( -- )
    0 0 0 0 0044 SYSCALL 4DROP ;

: VDP! ( data register -- )
    8 LSHIFT + >R
    0 R> 0 0 0047 SYSCALL 4DROP ;

: VRAM@ ( address -- data )
    >R 0 0 0 R> 004A SYSCALL 3DROP ;

: VRAM! ( data address -- )
    >R 0 0 R> 004D SYSCALL 4DROP ;

: FILL-VRAM ( data length address -- )
    0 SWAP 0056 SYSCALL 4DROP ;

: VRAM-TO-RAM ( length destination source -- )
    >R >R >R 0 R> R> R> 0059 SYSCALL 4DROP ;

: RAM-TO-VRAM ( length destination source -- )
    >R >R >R 0 R> R> R> 005C SYSCALL 4DROP ;

: CLEAR-SPRITES ( -- )
    0 0 0 0 0069 SYSCALL 4DROP ;

: CURSOR-POSITION ( column row -- )
    8 LSHIFT + >R
    0 0 0 R> 00C6 SYSCALL 4DROP ;

: CHANGE-COLOR ( bordercolor backgroundcolor foregroundcolor -- )
    FORCLR C! BAKCLR C! BDRCLR C!
    0 0 0 0 0062 SYSCALL 4DROP ;

: INIT-TEXT ( -- )
    0 0 0 0 006C SYSCALL 4DROP ;

: INIT-MODE32 ( -- )
    0 0 0 0 006F SYSCALL 4DROP ;

: INIT-GRAPH ( -- )
    0 0 0 0 0072 SYSCALL 4DROP ;

: INIT-MULTI ( -- )
    0 0 0 0 0075 SYSCALL 4DROP ;

: SPRITE-PATTERN ( spriteid -- address )
    0 0 0 0084 SYSCALL NIP NIP NIP ;

: SPRITE-ATTRIBUTE ( spriteid -- address )
    0 0 0 0087 SYSCALL NIP NIP NIP ;

: SPRITE-SIZE ( -- size )
    0 0 0 0 008A SYSCALL 3DROP ;

DEC
