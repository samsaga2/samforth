all: samc test.rom

samc: samc.c
	gcc -g samc.c -o samc -std=c99

test.rom: test.asm
	~/builds/sjasm42b8/sjasm -j test.asm test.rom

test.asm: test.fs samc samforth.begin samforth.end
	./samc test.fs > test.asm

test2.rom: test2.asm
	~/builds/sjasm42b8/sjasm -j test2.asm test2.rom

test2.asm: test2.fs samc samforth.begin samforth.end
	./samc test2.fs > test2.asm

test: test.rom
	openmsx -cart test.rom

test2: test2.rom
	openmsx -cart test2.rom

clear:
	rm -f test.asm test.lst test.rom samc

