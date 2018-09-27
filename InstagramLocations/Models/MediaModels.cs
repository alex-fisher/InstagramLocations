using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instacram.Models.MediaModels
{
    public class Dimensions
    {
        public int height { get; set; }
        public int width { get; set; }
    }

    public class DashInfo
    {
        public bool is_dash_eligible { get; set; }
        public object video_dash_manifest { get; set; }
        public int number_of_qualities { get; set; }
    }

    public class EdgeMediaToTaggedUser
    {
        public List<object> edges { get; set; }
    }

    public class Node
    {
        public string text { get; set; }
    }

    public class Edge
    {
        public Node node { get; set; }
    }

    public class EdgeMediaToCaption
    {
        public List<Edge> edges { get; set; }
    }

    public class PageInfo
    {
        public bool has_next_page { get; set; }
        public string end_cursor { get; set; }
    }

    public class Owner
    {
        public string id { get; set; }
        public string profile_pic_url { get; set; }
        public string username { get; set; }
    }

    public class Node2
    {
        public string id { get; set; }
        public string text { get; set; }
        public int created_at { get; set; }
        public Owner owner { get; set; }
    }

    public class Edge2
    {
        public Node2 node { get; set; }
    }

    public class EdgeMediaToComment
    {
        public int count { get; set; }
        public PageInfo page_info { get; set; }
        public List<Edge2> edges { get; set; }
    }

    public class Node3
    {
        public string id { get; set; }
        public string profile_pic_url { get; set; }
        public string username { get; set; }
    }

    public class Edge3
    {
        public Node3 node { get; set; }
    }

    public class EdgeMediaPreviewLike
    {
        public int count { get; set; }
        public List<Edge3> edges { get; set; }
    }

    public class EdgeMediaToSponsorUser
    {
        public List<object> edges { get; set; }
    }

    public class Owner2
    {
        public string id { get; set; }
        public string profile_pic_url { get; set; }
        public string username { get; set; }
        public bool blocked_by_viewer { get; set; }
        public bool followed_by_viewer { get; set; }
        public string full_name { get; set; }
        public bool has_blocked_viewer { get; set; }
        public bool is_private { get; set; }
        public bool is_unpublished { get; set; }
        public bool is_verified { get; set; }
        public bool requested_by_viewer { get; set; }
        public string profile_url => $@"https://www.instagram.com/{username}";
    }

    public class Node4
    {
        public string shortcode { get; set; }
        public string thumbnail_src { get; set; }
    }

    public class Edge4
    {
        public Node4 node { get; set; }
    }

    public class EdgeWebMediaToRelatedMedia
    {
        public List<Edge4> edges { get; set; }
    }

    public class ShortcodeMedia
    {
        public string __typename { get; set; }
        public string id { get; set; }
        public string shortcode { get; set; }
        public Dimensions dimensions { get; set; }
        public object gating_info { get; set; }
        public string media_preview { get; set; }
        public string display_url { get; set; }
        public List<object> display_resources { get; set; }
        public DashInfo dash_info { get; set; }
        public string video_url { get; set; }
        public int video_view_count { get; set; }
        public bool is_video { get; set; }
        public bool should_log_client_event { get; set; }
        public string tracking_token { get; set; }
        public EdgeMediaToTaggedUser edge_media_to_tagged_user { get; set; }
        public EdgeMediaToCaption edge_media_to_caption { get; set; }
        public bool caption_is_edited { get; set; }
        public EdgeMediaToComment edge_media_to_comment { get; set; }
        public bool comments_disabled { get; set; }
        public int taken_at_timestamp { get; set; }
        public EdgeMediaPreviewLike edge_media_preview_like { get; set; }
        public EdgeMediaToSponsorUser edge_media_to_sponsor_user { get; set; }
        public object location { get; set; }
        public bool viewer_has_liked { get; set; }
        public bool viewer_has_saved { get; set; }
        public bool viewer_has_saved_to_collection { get; set; }
        public Owner2 owner { get; set; }
        public bool is_ad { get; set; }
        public EdgeWebMediaToRelatedMedia edge_web_media_to_related_media { get; set; }
    }

    public class Graphql
    {
        public ShortcodeMedia shortcode_media { get; set; }
    }

    public class Media
    {
        public Graphql graphql { get; set; }
    }
}
