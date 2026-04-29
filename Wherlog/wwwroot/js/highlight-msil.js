/*
Language: MSIL
Author: where where <wherewhere7@outlook.com>
Description: Microsoft Intermediate Language
Category: assembler
*/

/** @type {import("highlight.js").LanguageFn} */
function LanguageFn(hljs) {
    const DECL_KEYWORDS = [
        '.addon',
        '.assembly',
        '.class',
        '.corflags',
        '.custom',
        '.data',
        '.emitbyte',
        '.entrypoint',
        '.event',
        '.export',
        '.field',
        '.file',
        '.fire',
        '.get',
        '.hash',
        '.imagebase',
        '.interfaceimpl',
        '.language',
        '.line',
        '.locale',
        '.locals',
        '.maxstack',
        '.method',
        '.module',
        '.mresource',
        '.mscorlib',
        '.namespace',
        '.other',
        '.override',
        '.pack',
        '.param',
        '.permission',
        '.permissionset',
        '.property',
        '.publickey',
        '.publickeytoken',
        '.removeon',
        '.set',
        '.size',
        '.stackreserve',
        '.subsystem',
        '.try',
        '.typedef',
        '.typelist',
        '.ver',
        '.vtable',
        '.vtentry',
        '.vtfixup',
        '.zeroinit'
    ];

    const BUILT_IN_KEYWORDS = [
        'array',
        'blob',
        'blob_object',
        'bool',
        'bstr',
        'byvalstr',
        'carray',
        'cf',
        'char',
        'class',
        'clsid',
        'currency',
        'custom',
        'date',
        'decimal',
        'error',
        'filetime',
        'float32',
        'float64',
        'hresult',
        'idispatch',
        'interface',
        'int',
        'int8',
        'int16',
        'int32',
        'int64',
        'iunknown',
        'lpstr',
        'lpstruct',
        'lptstr',
        'lpwstr',
        'native',
        'object',
        'objectref',
        'pinned',
        'record',
        'safearray',
        'storage',
        'stream',
        'streamed_object',
        'stored_object',
        'string',
        'struct',
        'syschar',
        'sysstring',
        'tbstr',
        'typedref',
        'uint',
        'uint8',
        'uint16',
        'uint32',
        'uint64',
        'unsigned',
        'userdefined',
        'valuetype',
        'variant',
        'void'
    ];

    const ATTR_KEYWORDS = [
        'abstract',
        'aggressiveinlining',
        'aggressiveoptimization',
        'amd64',
        'ansi',
        'arm',
        'arm64',
        'assembly',
        'async',
        'auto',
        'autochar',
        'beforefieldinit',
        'bestfit',
        'byreflike',
        'callconv',
        'cdecl',
        'charmaperror',
        'cil',
        'default',
        'enum',
        'explicit',
        'extended',
        'famandassem',
        'family',
        'famorassem',
        'fastcall',
        'final',
        'flags',
        'forwarder',
        'forwardref',
        'hidebysig',
        'import',
        'in',
        'initonly',
        'instance',
        'interface',
        'internalcall',
        'lasterr',
        'legacy',
        'library',
        'literal',
        'managed',
        'marshal',
        'nested',
        'newslot',
        'noinlining',
        'nomangle',
        'nometadata',
        'nooptimization',
        'noplatform',
        'off',
        'on',
        'optil',
        'opt',
        'out',
        'pinvokeimpl',
        'preservesig',
        'private',
        'privatescope',
        'public',
        'reqsecobj',
        'retargetable',
        'rtspecialname',
        'runtime',
        'sealed',
        'sequential',
        'serializable',
        'specialname',
        'static',
        'stdcall',
        'strict',
        'synchronized',
        'thiscall',
        'unmanaged',
        'unmanagedexp',
        'unicode',
        'value',
        'valuetype',
        'vararg',
        'virtual',
        'winapi',
        'windowsruntime',
        'x86'
    ];

    const ATTR_EXTRA_KEYWORDS = [
        'extends',
        'implements',
        'with',
        'to',
        'handler'
    ];

    const LITERAL_KEYWORDS = [
        'false',
        'null',
        'nullref',
        'true'
    ];

    const CONTEXTUAL_KEYWORDS = [
        '.ctor',
        '.cctor',
        'init',
        'as',
        'method',
        'type',
        'constraint',
        'catch',
        'filter',
        'finally',
        'fault',
        'tls',
    ];

    const META_KEYWORDS = [
        'define',
        'else',
        'endif',
        'ifdef',
        'ifndef',
        'include',
        'line',
        'undef'
    ];

    const OTHER_KEYWORDS = [
        '.this',
        '.base',
        '.nester',
        'algorithm',
        'alignment',
        'at',
        'bytearray',
        'extern',
        'modreq',
        'modopt',
        'field',
        'fixed',
        'notserialized',
        'any',
        'assert',
        'callmostderived',
        'demand',
        'deny',
        'fromunmanaged',
        'inheritcheck',
        'linkcheck',
        'mdtoken',
        'noncasdemand',
        'noncasinheritance',
        'noncaslinkdemand',
        'permitonly',
        'prejitdeny',
        'prejitgrant',
        'property',
        'reqmin',
        'reqopt',
        'reqrefuse',
        'request',
        'retainappdomain',
    ];

    const CLASS_MODIFIERS = [
        ATTR_KEYWORDS[0],   // abstract
        ATTR_KEYWORDS[4],   // ansi
        ATTR_KEYWORDS[7],   // assembly
        ATTR_KEYWORDS[9],   // auto
        ATTR_KEYWORDS[10],  // autochar
        ATTR_KEYWORDS[11],  // beforefieldinit
        ATTR_KEYWORDS[19],  // enum
        ATTR_KEYWORDS[20],  // explicit
        ATTR_KEYWORDS[21],  // extended
        ATTR_KEYWORDS[22],  // famandassem
        ATTR_KEYWORDS[23],  // family
        ATTR_KEYWORDS[24],  // famorassem
        ATTR_KEYWORDS[27],  // flags
        ATTR_KEYWORDS[31],  // import
        ATTR_KEYWORDS[35],  // interface
        ATTR_KEYWORDS[43],  // nested
        ATTR_KEYWORDS[57],  // private
        ATTR_KEYWORDS[59],  // public
        ATTR_KEYWORDS[62],  // rtspecialname
        ATTR_KEYWORDS[64],  // sealed
        ATTR_KEYWORDS[65],  // sequential
        ATTR_KEYWORDS[66],  // serializable
        ATTR_KEYWORDS[67],  // specialname
        ATTR_KEYWORDS[75],  // unicode
        ATTR_KEYWORDS[76],  // value
        ATTR_KEYWORDS[81],  // windowsruntime
        ATTR_EXTRA_KEYWORDS[0], // extends
        ATTR_EXTRA_KEYWORDS[1], // implements
        ATTR_EXTRA_KEYWORDS[2], // with
        ATTR_EXTRA_KEYWORDS[3], // to
        ATTR_EXTRA_KEYWORDS[4]  // handler
    ];

    const METHOD_MODIFIERS = [
        ATTR_KEYWORDS[0],   // abstract
        ATTR_KEYWORDS[7],   // assembly
        ATTR_KEYWORDS[22],  // famandassem
        ATTR_KEYWORDS[23],  // family
        ATTR_KEYWORDS[24],  // famorassem
        ATTR_KEYWORDS[26],  // final
        ATTR_KEYWORDS[27],  // flags
        ATTR_KEYWORDS[30],  // hidebysig
        ATTR_KEYWORDS[44],  // newslot
        ATTR_KEYWORDS[55],  // pinvokeimpl
        ATTR_KEYWORDS[57],  // private
        ATTR_KEYWORDS[58],  // privatescope
        ATTR_KEYWORDS[59],  // public
        ATTR_KEYWORDS[60],  // reqsecobj
        ATTR_KEYWORDS[62],  // rtspecialname
        ATTR_KEYWORDS[67],  // specialname
        ATTR_KEYWORDS[68],  // static
        ATTR_KEYWORDS[70],  // strict
        ATTR_KEYWORDS[71],  // synchronized
        ATTR_KEYWORDS[74],  // unmanagedexp
        ATTR_KEYWORDS[79]   // virtual
    ];

    const OP_CODES = [
        // instr_none
        'nop', 'break', 'ldarg.0', 'ldarg.1', 'ldarg.2', 'ldarg.3', 'ldloc.0', 'ldloc.1', 'ldloc.2', 'ldloc.3', 'stloc.0', 'stloc.1', 'stloc.2', 'stloc.3', 'ldnull',
        'ldc.i4.m1', 'ldc.i4.0', 'ldc.i4.1', 'ldc.i4.2', 'ldc.i4.3', 'ldc.i4.4', 'ldc.i4.5', 'ldc.i4.6', 'ldc.i4.7', 'ldc.i4.8',
        'dup', 'pop', 'ret',
        'ldind.i1', 'ldind.u1', 'ldind.i2', 'ldind.u2', 'ldind.i4', 'ldind.u4', 'ldind.i8', 'ldind.i', 'ldind.r4', 'ldind.r8', 'ldind.ref',
        'stind.ref', 'stind.i1', 'stind.i2', 'stind.i4', 'stind.i8', 'stind.r4', 'stind.r8',
        'add', 'sub', 'mul', 'div', 'div.un', 'rem', 'rem.un', 'and', 'or', 'xor', 'shl', 'shr', 'shr.un', 'neg', 'not',
        'conv.i1', 'conv.i2', 'conv.i4', 'conv.i8', 'conv.r4', 'conv.r8', 'conv.u4', 'conv.u8', 'conv.r.un', 'throw',
        'conv.ovf.i1.un', 'conv.ovf.i2.un', 'conv.ovf.i4.un', 'conv.ovf.i8.un', 'conv.ovf.u1.un', 'conv.ovf.u2.un', 'conv.ovf.u4.un', 'conv.ovf.u8.un', 'conv.ovf.i.un', 'conv.ovf.u.un',
        'ldlen', 'ldelem.i1', 'ldelem.u1', 'ldelem.i2', 'ldelem.u2', 'ldelem.i4', 'ldelem.u4', 'ldelem.i8', 'ldelem.i', 'ldelem.r4', 'ldelem.r8', 'ldelem.ref',
        'stelem.i', 'stelem.i1', 'stelem.i2', 'stelem.i4', 'stelem.i8', 'stelem.r4', 'stelem.r8', 'stelem.ref',
        'conv.ovf.i1', 'conv.ovf.u1', 'conv.ovf.i2', 'conv.ovf.u2', 'conv.ovf.i4', 'conv.ovf.u4', 'conv.ovf.i8', 'conv.ovf.u8',
        'ckfinite', 'conv.u2', 'conv.u1', 'conv.i', 'conv.ovf.i', 'conv.ovf.u',
        'add.ovf', 'add.ovf.un', 'mul.ovf', 'mul.ovf.un', 'sub.ovf', 'sub.ovf.un',
        'endfinally', 'stind.i', 'conv.u', 'prefix7', 'prefix6', 'prefix5', 'prefix4', 'prefix3', 'prefix2', 'prefix1', 'prefixref',
        'arglist', 'ceq', 'cgt', 'cgt.un', 'clt', 'clt.un', 'localloc', 'endfilter', 'volatile.', 'tail.', 'cpblk', 'initblk', 'rethrow', 'refanytype', 'readonly.', 'illegal', 'endmac',
        // instr_var
        'ldarg.s', 'ldarga.s', 'starg.s', 'ldloc.s', 'ldloca.s', 'stloc.s', 'ldarg', 'ldarga', 'starg', 'ldloc', 'ldloca', 'stloc',
        // instr_i
        'ldc.i4.s', 'ldc.i4', 'unaligned.', 'no.',
        // instr_i8
        'ldc.i8',
        // instr_r
        'ldc.r4', 'ldc.r8',
        // instr_brtarget
        'br.s', 'brfalse.s', 'brtrue.s', 'beq.s', 'bge.s', 'bgt.s', 'ble.s', 'blt.s', 'bne.un.s', 'bge.un.s', 'bgt.un.s', 'ble.un.s', 'blt.un.s',
        'br', 'brfalse', 'brtrue', 'beq', 'bge', 'bgt', 'ble', 'blt', 'bne.un', 'bge.un', 'bgt.un', 'ble.un', 'blt.un', 'leave', 'leave.s',
        // instr_method
        'jmp', 'call', 'callvirt', 'newobj', 'ldftn', 'ldvirtftn',
        // instr_field
        'ldfld', 'ldflda', 'stfld', 'ldsfld', 'ldsflda', 'stsfld',
        // instr_type
        'cpobj', 'ldobj', 'castclass', 'isinst', 'unbox', 'stobj', 'box', 'newarr', 'ldelema', 'ldelem', 'stelem', 'unbox.any', 'refanyval', 'mkrefany', 'initobj', 'constrained.', 'sizeof',
        // instr_string
        'ldstr',
        // instr_sig
        'calli',
        // instr_tok
        'ldtoken',
        // instr_switch
        'switch'
    ];

    const KEYWORDS = {
        $pattern: "\\.?[a-zA-Z][\\.\\w\\d]*",
        keyword: DECL_KEYWORDS.concat(ATTR_KEYWORDS, ATTR_EXTRA_KEYWORDS, CONTEXTUAL_KEYWORDS, OTHER_KEYWORDS),
        built_in: BUILT_IN_KEYWORDS.concat(OP_CODES),
        literal: LITERAL_KEYWORDS
    };
    const TITLE_MODE = hljs.inherit(hljs.TITLE_MODE, { begin: '[a-zA-Z](\\.?\\w)*' });
    const NUMBERS = hljs.C_NUMBER_MODE;
    const STRING = {
        variants: [
            hljs.APOS_STRING_MODE,
            hljs.QUOTE_STRING_MODE
        ]
    };

    const GENERIC_MODIFIER = {
        begin: '<',
        end: '>',
        contains: [
            {
                className: 'keyword',
                match: /(\+|-|class|valuetype|\.ctor)/
            },
            {
                begin: '\\(',
                end: '\\)',
                keywords: KEYWORDS
            },
            TITLE_MODE
        ]
    };

    return {
        name: 'IL',
        aliases: [
            'il',
            'msil',
            'cil'
        ],
        keywords: KEYWORDS,
        contains: [
            hljs.C_LINE_COMMENT_MODE,
            hljs.C_BLOCK_COMMENT_MODE,
            {
                className: 'meta',
                begin: '#',
                end: '$',
                keywords: { keyword: META_KEYWORDS }
            },
            STRING,
            NUMBERS,
            {
                className: 'class',
                begin: /\.class/,
                end: '{',
                excludeEnd: true,
                keywords: KEYWORDS,
                contains: [
                    {
                        beginKeywords: CLASS_MODIFIERS.join(' '),
                        relevance: 0
                    },
                    hljs.APOS_STRING_MODE,
                    {
                        begin: hljs.IDENT_RE + '\\s*(<[^=]+>)?',
                        returnBegin: true,
                        contains: [
                            hljs.TITLE_MODE,
                            GENERIC_MODIFIER
                        ],
                        relevance: 0
                    },
                    hljs.C_LINE_COMMENT_MODE
                ]
            },
            {
                className: 'function',
                begin: /\.method/,
                end: '{',
                excludeEnd: true,
                keywords: KEYWORDS,
                contains: [
                    // prevents these from being highlighted `title`
                    {
                        beginKeywords: METHOD_MODIFIERS.join(' '),
                        relevance: 0
                    },
                    hljs.APOS_STRING_MODE,
                    {
                        begin: hljs.IDENT_RE + '\\s*(<[^=]+>\\s*)?\\(',
                        returnBegin: true,
                        contains: [
                            hljs.TITLE_MODE,
                            GENERIC_MODIFIER
                        ],
                        relevance: 0
                    },
                    hljs.C_LINE_COMMENT_MODE
                ]
            },
            {
                begin: /=|bytearray\s+\(/,
                end: '\\)',
                excludeBegin: true,
                excludeEnd: true,
                keywords: KEYWORDS,
                contains: [
                    {
                        className: 'number',
                        match: /[\da-fA-F]{2}/,
                    }
                ]
            }
        ]
    };
}

export default LanguageFn;