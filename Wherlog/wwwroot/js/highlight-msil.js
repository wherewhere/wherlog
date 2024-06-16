/*
Language: MSIL
Author: where where <wherewhere7@outlook.com>
Description: Microsoft Intermediate Language
Category: assembler
*/

/** @type LanguageFn */
function LanguageFn(hljs) {
    const BUILT_IN_KEYWORDS = [
        'bool',
        'uint8',
        'char',
        'unsigned',
        'float64',
        'float32',
        'int32',
        'int64',
        'native',
        'object',
        'int8',
        'int16',
        'string',
        'uint64',
        'uint32',
        'uint16'
    ];
    const CLASS_MODIFIERS = [
        'public',
        'private',
        'value',
        'enum',
        'interface',
        'sealed',
        'abstract',
        'auto',
        'sequential',
        'explicit',
        'ansi',
        'unicode',
        'autochar',
        'import',
        'serializable',
        'windowsruntime',
        'nested',
        'family',
        'assembly',
        'famandassem',
        'famorassem',
        'beforefieldinit',
        'specialname',
        'rtspecialname',
        'flags'
    ];
    const FUNCTION_MODIFIERS = [
        'static',
        'public',
        'private',
        'family',
        'final',
        'specialname',
        'virtual',
        'strict',
        'abstract',
        'assembly',
        'famandassem',
        'famorassem',
        'privatescope',
        'hidebysig',
        'newslot',
        'rtspecialname',
        'unmanagedexp',
        'reqsecobj',
        'flags',
        'pinvokeimpl'
    ];
    const NORMAL_KEYWORDS = [
        // Start with '.'
        'subsystem',
        'corflags',
        'file', 'alignment',
        'imagebase',
        'stackreserve',
        'typelist',
        'mscorlib',
        'language',
        'typedef',
        'custom',
        'module',
        'vtfixup',
        'vtable',
        'namespace',
        'class',
        'size',
        'pack',
        'override',
        'interfaceimpl',
        'field',
        'event',
        'addon',
        'removeon',
        'fire',
        'other',
        'property',
        'set',
        'get',
        'method',
        'this',
        'base',
        'nester',
        'permission',
        'permissionset',
        'line',
        'file',
        'hash',
        'assembly',
        'ver',
        'publickey',
        'publickeytoken',
        'locale',
        'mresource',
        // Start without '.'
        'type',
        'enum',
        'extern',
        'fromunmanaged',
        'callmostderived',
        'retainappdomain',
        'public',
        'private',
        'value',
        'interface',
        'sealed',
        'abstract',
        'auto',
        'sequential',
        'explicit',
        'ansi',
        'unicode',
        'autochar',
        'import',
        'serializable',
        'windowsruntime',
        'nested',
        'beforefieldinit',
        'specialname',
        'rtspecialname',
        'flags',
        'extends',
        'implements',
        'param',
        'static',
        'family',
        'initonly',
        'marshal',
        'famandassem',
        'famorassem',
        'privatescope',
        'literal',
        'notserialized',
        'at',
        'mdtoken',
        'final',
        'virtual',
        'strict',
        'hidebysig',
        'newslot',
        'unmanagedexp',
        'reqsecobj',
        'pinvokeimpl',
        'as',
        'nomangle',
        'lasterr',
        'winapi',
        'bestfit',
        'on',
        'off',
        'charmaperror',
        'in',
        'out',
        'opt',
        'native',
        'cil',
        'optil',
        'managed',
        'forwardref',
        'preservesig',
        'runtime',
        'internalcall',
        'synchronized',
        'noinlining',
        'aggressiveinlining',
        'nooptimization',
        'tls', 'request',
        'demand',
        'assert',
        'deny',
        'permitonly',
        'linkcheck',
        'inheritcheck',
        'reqmin',
        'reqopt',
        'reqrefuse',
        'prejitgrant',
        'prejitdeny',
        'noncasdemand',
        'noncaslinkdemand',
        'noncasinheritance',
        'retargetable',
        'legacy', 'library',
        'x86',
        'ia64',
        'amd64',
        'arm',
        'void'
    ];
    const LITERAL_KEYWORDS = [
        'default',
        'nullref',
    ];
    const CONTEXTUAL_KEYWORDS = [
        // Start with '.'
        'ctor',
        'cctor',
        'locals',
        'emitbyte',
        'maxstack',
        'entrypoint',
        'zeroinit',
        'export',
        'vtentry',
        'try',
        'data',
        // Start without '.'
        'instance',
        'callconv',
        'vararg',
        'unmanaged',
        'cdecl',
        'stdcall',
        'thiscall',
        'fastcall',
        'filter',
        'catch',
        'finally',
        'fault',
        'handler'
    ]

    const KEYWORDS = {
        keyword: NORMAL_KEYWORDS.concat(CONTEXTUAL_KEYWORDS),
        built_in: BUILT_IN_KEYWORDS,
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
                begin: /(\\+|\\-|class|valuetype|\\.ctor)/
            },
            {
                begin: '\\(',
                end: '\\)'
            },
            TITLE_MODE
        ]
    };

    return {
        name: 'IL',
        aliases: [
            'il',
            'msil',
            'ilasm'
        ],
        keywords: KEYWORDS,
        contains: [
            hljs.COMMENT(
                '///',
                '$',
                {
                    returnBegin: true,
                    contains: [
                        {
                            className: 'doctag',
                            variants: [
                                {
                                    begin: '///',
                                    relevance: 0
                                },
                                { begin: '<!---->' },
                                {
                                    begin: '</?',
                                    end: '>'
                                }
                            ]
                        }
                    ]
                }
            ),
            hljs.C_LINE_COMMENT_MODE,
            STRING,
            NUMBERS,
            {
                begin: '.class',
                end: '{',
                excludeEnd: true,
                keywords: KEYWORDS,
                contains: [
                    {
                        beginKeywords: CLASS_MODIFIERS.join(' '),
                        relevance: 0
                    },
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
                className: 'function',
                begin: '.method',
                end: '{',
                excludeEnd: true,
                keywords: KEYWORDS,
                contains: [
                    // prevents these from being highlighted `title`
                    {
                        beginKeywords: FUNCTION_MODIFIERS.join(' '),
                        relevance: 0
                    },
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
            }
        ]
    };
}

hljs.registerLanguage('msil', LanguageFn);
