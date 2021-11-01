# Vue.cs

This framework provides full power and beauty of .NET and purity of Vue.js. Write your code in Vue.js style, but with C# programming language and compile it into WebAssembly.

Now the framework uses Blazor framework, because my skill in compiling is not usable. If you don't like it, you can join ;-)

Difference from Blazor:

* You don't have to care about manual re-render after data modification outside of component.
* DOM is changed only on that places, where data is changed. It doesn't re-render whole component.
* No unnecessery junk in your HTML part (_if_, _CascadingValue_, etc.).

Difference from Vue.js:

* C# programming language (no need of typescript anymore).
* As a store you can use any object with DependecyInjection.

## Motivation

I love .Net 5 and all the stuff around it. And I love Vue.js. When I first heard about Blazor I was so excited. But when I tried to use it, my excitement disapeared. Blazor has many behaviour that I don't like and it seems to me, that authors tried to copy React features (which I don't like - my personal view, sorry) instead of Vue. I started to think how to use C# in browser in some nicer way.

## How it works

This framework ignores most of Blazor features - uses only _C# to wasm_ compiler, JSInterop (adapter for JS - only way how to manipulate DOM) and serving server.

The framework does these steps:

1. Take your \*.vue-cs files and convert it into full C# code (\*.cs).
1. Compile generated code as a _Blazor wasm_ project.
1. Add some \*.js files for manipulating DOM, that generated code will use.
1. Run as a _Blazor wasm_ project.

## Get started

It is not usable yet, sorry. Working on it ;-)

## Component example - Counter.vue-cs

```html
<template>
    <div class="counter">
        <div class="value">
            Current count is <span>{{ Count }}</span>
        </div>
        <button @click="Increment">Add 1</button>
    </div>
</template>

<script>
public class Counter : BaseComponent
{
    public Counter(Store store)
    {
        _store = store;
    }

    private Store _store;

    public int Count => _store.Counter.Value;

    public void Increment()
    {
        _store.Increment();
    }
}
</script>

<style>
.counter {
    display: grid;
    justify-content: center;
    align-content: center;
}
</style>
```

## Develop progress

- [x] Find out manipulating with DOM
- [ ] Manually write "generated" code
- [ ] Create code generator
- [ ] Add all features from Vue.js
