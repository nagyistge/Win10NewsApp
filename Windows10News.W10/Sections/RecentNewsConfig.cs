


using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Rss;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;
using AppStudio.Uwp;
using System.Linq;
using Windows10News.Config;
using Windows10News.ViewModels;

namespace Windows10News.Sections
{
    public class RecentNewsConfig : SectionConfigBase<RssSchema>
    {
		public override Func<Task<IEnumerable<RssSchema>>> LoadDataAsyncFunc
        {
            get
            {
                var config = new RssDataConfig
                {
                    Url = new Uri("http://blogs.windows.com/feed/")
                };

                return () => Singleton<RssDataProvider>.Instance.LoadDataAsync(config, MaxRecords);
            }
        }

        public override ListPageConfig<RssSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<RssSchema>
                {
                    Title = "Recent news",

					PageTitle = "Recent news",

                    ListNavigationInfo = NavigationInfo.FromPage("RecentNewsListPage"),

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Title.ToSafeString();
                        viewModel.SubTitle = item.Summary.ToSafeString();
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.ImageUrl.ToSafeString());
                    },
                    DetailNavigation = (item) =>
                    {
                        return NavigationInfo.FromPage("RecentNewsDetailPage", true);
                    }
                };
            }
        }

        public override DetailPageConfig<RssSchema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, RssSchema>>();
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.Author.ToSafeString();
                    viewModel.Title = item.Title.ToSafeString();
                    viewModel.Description = item.Content.ToSafeString();
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl("");
                    viewModel.Content = null;
					viewModel.Source = item.FeedUrl;
                });

                var actions = new List<ActionConfig<RssSchema>>
                {
                    ActionConfig<RssSchema>.Link("Go To Source", (item) => item.FeedUrl.ToSafeString()),
                };

                return new DetailPageConfig<RssSchema>
                {
                    Title = "Recent news",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
    }
}
