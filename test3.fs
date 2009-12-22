INCLUDE kernel.fs
INCLUDE bios.fs

: show-keys ( -- )
    CHGET . cr recurse ;

: main
    cr
    ." Hello world!" cr
    decimal show-keys ;

